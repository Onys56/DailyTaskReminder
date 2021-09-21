using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using DailyTaskReminder.Reminders;

namespace Configurator
{
    /// <summary>
    /// Form for editing and creating reminders config file.
    /// </summary>
    public partial class RemindersForm : Form
    {
        /// <summary>
        /// Path to reminder config file
        /// </summary>
        private string reminderFileName;

        /// <summary>
        /// All types that implement <c cref="IReminder">reminder interface</c>.
        /// Loaded using reflection <see cref="LoadReminderTypes"/>
        /// </summary>
        private Type[] reminderTypes;

        /// <summary>
        /// Makes the event ignore the change in a textfield.
        /// </summary>
        private bool ignoreFieldChange;

        public RemindersForm(string reminderFileName)
        {
            InitializeComponent();

            this.reminderFileName = reminderFileName;
            LoadReminderTypes();
            LoadReminders();
        }

        /// <summary>
        /// Wrapper for reminder types.
        /// For overriting ToString.
        /// </summary>
        private class ReminderTypeListItem
        {
            /// <summary>
            /// Wrapped type.
            /// </summary>
            public Type Type;

            public ReminderTypeListItem(Type type)
            {
                Type = type;
            }

            public override string ToString()
            {
                return Type.Name;
            }
        }

        /// <summary>
        /// Wrapper for reminders.
        /// For overriting ToString and storing the name of the remuinder.
        /// </summary>
        private class ReminderListItem
        {
            /// <summary>
            /// Wrapped remidner.
            /// </summary>
            public IReminder Reminder;

            /// <summary>
            /// Name of the reminder.
            /// </summary>
            public string Name;
            public ReminderListItem(IReminder reminder, string name)
            {
                Reminder = reminder;
                Name = name;
            }

            public override string ToString()
            {
                return Name;
            }
        }

        /// <summary>
        /// Wrapper for FieldInfos of reminders.
        /// For overriting ToString.
        /// </summary>
        private class FieldListItem
        {
            /// <summary>
            /// Wrapped FieldInfo
            /// </summary>
            public FieldInfo Field;

            public FieldListItem(FieldInfo field)
            {
                Field = field;
            }

            public override string ToString()
            {
                return Field.Name;
            }
        }

        /// <summary>
        /// Loads reminder types using reflection.
        /// </summary>
        private void LoadReminderTypes()
        {
            Type Itype = typeof(IReminder);
            reminderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => Itype.IsAssignableFrom(t) && !t.IsInterface).ToArray();

            foreach (Type t in reminderTypes)
            {
                reminderTypes_listBox.Items.Add(new ReminderTypeListItem(t));
            }
        }

        /// <summary>
        /// Adds reminders to listBox to be displayed to the user.
        /// </summary>
        private void LoadReminders()
        {
            foreach (var item in Instances.GetReminderByName)
            {
                reminder_listBox.Items.Add(new ReminderListItem(item.Value, item.Key));
            }
        }

        /// <summary>
        /// Handles the click on cancel button.
        /// Closes the form.
        /// </summary>
        private void cancel_button_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Handles change in reminder listBox.
        /// Displays fields in the field listBox and copies the name into the textBox for possible change by user.
        /// </summary>
        private void reminder_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReminderListItem r = (ReminderListItem)reminder_listBox.SelectedItem;
            if (r is null)
            {
                fields_listBox.Items.Clear();
                return; 
            }

            fields_listBox.Items.Clear();
            foreach (var field in r.Reminder.GetType().GetFields())
            {
                fields_listBox.Items.Add(new FieldListItem(field));
            }
            ignoreFieldChange = true;
            field_textBox.Text = r.Name;
            ignoreFieldChange = false;
        }

        /// <summary>
        /// Handles the change in field listBox.
        /// Copies the field value into textBox so that the user can change it.
        /// </summary>
        private void fields_listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReminderListItem r = (ReminderListItem)reminder_listBox.SelectedItem;
            FieldListItem f = (FieldListItem)fields_listBox.SelectedItem;

            if (r is null || f is null) return;
            IReminder selected = r.Reminder;

            ignoreFieldChange = true;
            object value = f.Field.GetValue(selected);
            field_textBox.Text = value is null ? "" : value.ToString();
            ignoreFieldChange = false;
        }

        /// <summary>
        /// Handles change of text in textBox.
        /// Based on what is currently selected updates the coresponding value (name of task or any field)
        /// </summary>
        private void field_textBox_TextChanged(object sender, EventArgs e)
        {
            if (ignoreFieldChange) return;

            ReminderListItem selected = (ReminderListItem)reminder_listBox.SelectedItem;

            if (fields_listBox.SelectedItem != null)
            {
                FieldInfo field = ((FieldListItem)fields_listBox.SelectedItem).Field;
                field.SetValue(selected.Reminder, field_textBox.Text);
            }
            else if (reminder_listBox.SelectedItem != null)
            {
                string oldName = selected.Name;
                selected.Name = field_textBox.Text;
                Instances.GetReminderByName.Remove(oldName);
                Instances.GetReminderByName.Add(selected.Name, selected.Reminder);
                reminder_listBox.Items[reminder_listBox.SelectedIndex] = reminder_listBox.SelectedItem;
            }
        }

        /// <summary>
        /// Handles the click on save button.
        /// Saves the reminders into file.
        /// </summary>
        private void save_button_Click(object sender, EventArgs e)
        {
            Instances.SaveReminders(reminderFileName);
            MessageBox.Show("Save successful", "Save complete");
        }

        /// <summary>
        /// Handles the click on add reminder button.
        /// Creates new reminder of selected type and adds it to listBox.
        /// </summary>
        private void addReminder_button_Click(object sender, EventArgs e)
        {
            ReminderTypeListItem r = (ReminderTypeListItem)reminderTypes_listBox.SelectedItem;
            if (r is null)
            {
                MessageBox.Show("No reminder type selected", "Error");
                return;
            }

            IReminder reminder = Instances.CreateReminder(r.Type.Name);
            string name = $"{r.Type.Name} {Instances.GetReminderByName.Count}";
            Instances.GetReminderByName.Add(name, reminder);

            reminder_listBox.Items.Add(new ReminderListItem(reminder, name));
        }

        /// <summary>
        /// Handles the click on the delete reminder button.
        /// Removes the reminder from the listBox.
        /// </summary>
        private void deleteReminder_button_Click(object sender, EventArgs e)
        {
            ReminderListItem r = (ReminderListItem)reminder_listBox.SelectedItem;
            if (r is null)
            {
                MessageBox.Show("No reminder selected", "Error");
                return;
            }

            reminder_listBox.Items.Remove(r);
            Instances.GetReminderByName.Remove(r.Name);
        }
    }
}
