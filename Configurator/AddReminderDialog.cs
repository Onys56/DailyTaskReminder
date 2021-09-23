using System;
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
    /// Dialog for adding reminders to tasks.
    /// Has autocomplete support if the user loads the reminders (or if they were already loaded from the configuration)
    /// </summary>
    public partial class AddReminderDialog : Form
    {
        MainForm main;

        public AddReminderDialog(MainForm m)
        {
            InitializeComponent();
            UpdateRemindersStatus();
            main = m;
        }

        /// <summary>
        /// Adds the reminder to the task if the names is valid and doesn't already exist
        /// </summary>
        private void add_button_Click(object sender, EventArgs e)
        {
            string name = reminder_textBox.Text;
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Reminder name can not be empty", "Error");
                return;
            };

            if (main.selectedTask.Reminders.Contains(name))
            {
                MessageBox.Show("The task already has this reminder", "Error");
                return;
            }

            main.selectedTask.Reminders.Add(name);
            main.RefreshTaskDisplay();
            Close();
        }

        /// <summary>
        /// Updates the status of Reminders based on reminders in the static Instances class
        /// </summary>
        private void UpdateRemindersStatus()
        {
            if (Instances.GetReminderByName.Count > 0)
            {
                reminderStatus.Text = "Loaded";
                loadReminders_button.Hide();

                AutoCompleteStringCollection source = new();
                source.AddRange(Instances.GetReminderByName.Keys.ToArray());
                reminder_textBox.AutoCompleteCustomSource = source;
                reminder_textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                reminder_textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;

            }
            else
            {
                reminderStatus.Text = "Not Loaded";
                loadReminders_button.Show();

                reminder_textBox.AutoCompleteMode = AutoCompleteMode.None;
            }
        }

        /// <summary>
        /// Loads the reminders from reminder config file
        /// </summary>
        private void loadReminders_button_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string reminderFile = openFileDialog1.FileName;
                Instances.LoadReminders(reminderFile);
                UpdateRemindersStatus();
            }
        }

        /// <summary>
        /// Closes the form
        /// </summary>
        private void cancel_button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}