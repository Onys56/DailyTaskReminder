using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualBasic;

using DailyTaskReminder.Tasks;
using DailyTaskReminder.Reminders;

namespace Configurator
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// List of loaded tasks
        /// </summary>
        public List<Task> Tasks;

        /// <summary>
        /// Path to config file where Tasks will be stored
        /// </summary>
        private string filePath;

        // Groups of UI controls for easier hiding and showing
        /// <summary>
        /// Controls in the main menu
        /// </summary>
        private Control[] menuControls;
        /// <summary>
        /// Controls in the config screen
        /// </summary>
        private Control[] configControls;
        /// <summary>
        /// Controls only shown for some types of Tasks
        /// </summary>
        private Control[] specialConfig;

        /// <summary>
        /// The task that is currently selected in the <c cref="TaskList">list of tasks</c>
        /// </summary>
        private Task selectedTask;

        public MainForm()
        {
            InitializeComponent();

            menuControls = new Control[]
            {
                button_existing_file,
                button_new_file,
                newReminder_button,
                existingRemider_button
            };

            configControls = new Control[]
            {
                TaskList,
                button_back_to_menu,
                dateTimePicker1,
                taskList_label,
                dueTime_label,
                taskName_label,
                taskName_textBox,
                dateTimePicker2,
                remindTime_label,
                reminders_label,
                reminders_listBox,
                remindersAdd_button,
                remindersDelete_button,
                saveToFile_button,
                addTask_button,
                deleteTask_Button
            };

            specialConfig = new Control[]
            {
                weekDays_label,
                weekDays_checkBox,
                monthDay_label,
                month_label,
                month_number,
                day_number,
                reminderDays_label,
                reminderDays_number
                
            };

            foreach (var c in configControls)
            {
                c.Hide();
            }

            foreach (var c in specialConfig)
            {
                c.Hide();
            }
        }

        /// <summary>
        /// Wrapper for tasks with overriden ToString method
        /// </summary>
        class TaskListItem
        {
            public Task Task;
            public TaskListItem(Task task)
            {
                this.Task = task;
            }

            public override string ToString()
            {
                return Task.Name;
            }
        }

        /// <summary>
        /// Loads the tasks from selected file
        /// </summary>
        private void button_existing_file_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "Tasks.txt";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                Tasks = Serialization.Deserialize(filePath);
                ChangeToConfig();
            }
        }

        /// <summary>
        /// Lets the user specify where the config file should be saved
        /// </summary>
        private void button_new_file_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Tasks.txt";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                filePath = saveFileDialog1.FileName;
                Tasks = new();
                ChangeToConfig();
            }
        }

        /// <summary>
        /// Change to the configuration "scene".
        /// Hides all the menu controls and shows the common configuration controls.
        /// The special configuration controls remain hidden as they should be visible only for some tasks.
        /// </summary>
        private void ChangeToConfig()
        {
            foreach (var c in menuControls)
            {
                c.Hide();
            }

            foreach (var c in configControls)
            {
                c.Show();
            }

            selectedTask = null;
            TaskList.Items.Clear();

            foreach (Task task in Tasks)
            {
                TaskList.Items.Add(new TaskListItem(task));
            }
        }

        /// <summary>
        /// Chacnge back to main memu.
        /// </summary>
        private void ChangeToMenu()
        {
            foreach (var c in configControls)
            {
                c.Hide();
            }

            foreach (var c in specialConfig)
            {
                c.Hide();
            }

            foreach (var c in menuControls)
            {
                c.Show();
            }
        }

        /// <summary>
        /// When user selects different task in the <c cref="TaskList">list of tasks</c> this method
        /// changes the <c cref="selectedTask">selected task field</c>.
        /// </summary>
        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TaskList.SelectedItem is null) return;
            selectedTask = ((TaskListItem)TaskList.SelectedItem).Task;
            RefreshTaskDisplay();
        }

        /// <summary>
        /// Fills in the properties of selected task to the UI controls where the user can see and change them.
        /// </summary>
        private void RefreshTaskDisplay()
        {
            taskName_textBox.Text = selectedTask.Name;

            DateTime oldD = dateTimePicker1.Value;
            DateTimeOffset newD = ((SimpleTask)selectedTask).DueTime;
            dateTimePicker1.Value = new DateTime(oldD.Year, oldD.Month, oldD.Day, newD.Hour, newD.Minute, newD.Second);

            ignoreReminderTime = true;
            oldD = dateTimePicker2.Value;
            TimeSpan newT = selectedTask.RemindSpan;
            dateTimePicker2.Value = new DateTime(oldD.Year, oldD.Month, oldD.Day, newT.Hours, newT.Minutes, newT.Seconds);
            reminderDays_number.Value = newT.Days;
            ignoreReminderTime = false;

            reminders_listBox.Items.Clear();
            foreach (string reminder in selectedTask.Reminders)
            {
                reminders_listBox.Items.Add(reminder);
            }

            foreach (var c in specialConfig)
            {
                c.Hide();
            }

            switch (selectedTask.GetType().Name)
            {
                case "WeeklyTask":
                    weekDays_label.Show();
                    weekDays_checkBox.Show();
                    reminderDays_label.Show();
                    reminderDays_number.Show();

                    ignoreCheck = true;
                    foreach (int i in weekDays_checkBox.CheckedIndices)
                    {
                        weekDays_checkBox.SetItemChecked(i, false);
                    }
                    foreach (DayOfWeek day in ((WeeklyTask)selectedTask).Days)
                    {
                        weekDays_checkBox.SetItemChecked((6 + (int)day) % 7, true);
                    }
                    ignoreCheck = false;

                    reminderDays_number.Value = selectedTask.RemindSpan.Days;
                    break;
                case "MonthlyTask":
                    monthDay_label.Show();
                    day_number.Show();
                    reminderDays_label.Show();
                    reminderDays_number.Show();

                    day_number.Value = ((MonthlyTask)selectedTask).Day;
                    reminderDays_number.Value = selectedTask.RemindSpan.Days;
                    break;
                case "YearlyTask":
                    monthDay_label.Show();
                    month_label.Show();
                    day_number.Show();
                    month_number.Show();
                    reminderDays_label.Show();
                    reminderDays_number.Show();

                    day_number.Value = ((YearlyTask)selectedTask).Day;
                    month_number.Value = ((YearlyTask)selectedTask).Month;
                    reminderDays_number.Value = selectedTask.RemindSpan.Days;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Back to menu button with confirmation.
        /// </summary>
        private void button_back_to_menu_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to go back? Any unsaved changes will be lost.", "Confirm", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes) ChangeToMenu();
        }

        /// <summary>
        /// Add reminder to a task.
        /// </summary>
        private void remindersAdd_button_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItem == null) return;
            string name = Interaction.InputBox("Name of the reminder:\n(Use the same name that is defined in the reminders config file)", "Add reminder");
            if (string.IsNullOrWhiteSpace(name)) return;

            selectedTask.Reminders.Add(name);
            RefreshTaskDisplay();
        }

        /// <summary>
        /// Remove reminder from a task.
        /// </summary>
        private void remindersDelete_button_Click(object sender, EventArgs e)
        {
            string selected = (string)reminders_listBox.SelectedItem;
            if (selected is null) return;

            selectedTask.Reminders.Remove(selected);
            RefreshTaskDisplay();
        }

        /// <summary>
        /// Display error to user.
        /// </summary>
        private void ShowError(string message)
        {
            MessageBox.Show(message, "Error");
        }

        /// <summary>
        /// Handles the change in day number text box.
        /// </summary>
        private void day_number_ValueChanged(object sender, EventArgs e)
        {
            if(selectedTask.GetType().Name == "MonthlyTask")
            {
                ((MonthlyTask)selectedTask).Day = (int)day_number.Value;
            }
            else if (selectedTask.GetType().Name == "YearlyTask")
            {
                ((YearlyTask)selectedTask).Day = (int)day_number.Value;
            }
        }

        /// <summary>
        /// Handles the change in month number text box.
        /// </summary>
        private void month_number_ValueChanged(object sender, EventArgs e)
        {
            ((YearlyTask)selectedTask).Month = (int)month_number.Value;
        }
        /// <summary>
        /// Ignore the <c cref="weekDays_checkBox_ItemCheck(object, ItemCheckEventArgs)">checkBox checked event</c>.
        /// Useful when changing the check status in code so that the event does not get triggered.
        /// </summary>
        private bool ignoreCheck;

        /// <summary>
        /// Handles cheking and unchecking of checkBox list items.
        /// </summary>
        private void weekDays_checkBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ignoreCheck) return;
            List<DayOfWeek> days = ((WeeklyTask)selectedTask).Days;
            DayOfWeek day = (DayOfWeek)((1 + e.Index) % 7);

            if (e.NewValue == CheckState.Checked)
            {
                if (!days.Contains(day)) ((WeeklyTask)selectedTask).Days.Add(day);
                UpdateReminderTime();
            }
            else
            {
                if (days.Contains(day)) days.Remove((DayOfWeek)((1 + e.Index) % 7));
            }
        }

        /// <summary>
        /// Changes the <c cref="Task.RemindSpan">time before reminder</c> of selected task.
        /// </summary>
        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreReminderTime) return;
            if (selectedTask is null) return;
            UpdateReminderTime();
        }

        /// <summary>
        /// Changes the <c cref="SimpleTask.DueTime">deadline time</c> of selected task.
        /// </summary>
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (selectedTask is null) return;
            ((SimpleTask)selectedTask).DueTime = dateTimePicker1.Value;
        }

        /// <summary>
        /// Changes the name of the task.
        /// </summary>
        private void taskName_textBox_TextChanged(object sender, EventArgs e)
        {
            if (selectedTask is null) return;
            string newName = taskName_textBox.Text;
            if (Tasks.Count(t => t.Name == newName) >= 2)
            {
                ShowError($"Tasks can't have the same name: {newName}");
                return;
            }
            else
            {
                selectedTask.Name = newName;
                int i = TaskList.SelectedIndex;
                TaskList.Items[i] = TaskList.Items[i];
            }
        }

        /// <summary>
        /// Handles the click on Add Task button.
        /// Shows dialog using the <c cref="NewTaskDialog">dialog form</c>.
        /// </summary>
        private void addTask_button_Click(object sender, EventArgs e)
        {
            Form dialog = new NewTaskDialog(this);
            dialog.ShowDialog();
        }

        /// <summary>
        /// Create a now task and add it the the list.
        /// </summary>
        public void AddTask(Task t)
        {
            Tasks.Add(t);
            TaskList.Items.Add(new TaskListItem(t));
        }

        /// <summary>
        /// Prevents beep when pressing enter in the <c cref="taskName_textBox">task name textBox</c>
        /// and instead switches focus to the next user input (the deadline picker).
        /// </summary>
        private void taskName_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dateTimePicker1.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;

            }

        }

        /// <summary>
        /// Handles the delete task button.
        /// Asks the user for conformation.
        /// </summary>
        private void deleteTask_Button_Click(object sender, EventArgs e)
        {
            if (TaskList.SelectedItem is null) return;
            var confirmResult = MessageBox.Show($"Are you sure you want to delete task: {selectedTask.Name}?", "Confirm", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                TaskListItem t = (TaskListItem)TaskList.SelectedItem;
                TaskList.Items.Remove(t);
                Tasks.Remove(t.Task);
            }
        }

        /// <summary>
        /// Saves the tasks to the <c cref="filePath">pre-specified file</c>.
        /// </summary>
        private void saveToFile_button_Click(object sender, EventArgs e)
        {
            try
            {
                Validation.ValidateTasks(Tasks);
                Serialization.Serialize(Tasks, filePath);
                MessageBox.Show("Save successful", "Save complete");
            }
            catch (TasksNotValidException er)
            {
                ShowError("Tasks are not valid, please fix the error:\n" + er.Message);
            }

        }

        /// <summary>
        /// Lets the user specify where to save the remind coinfig file 
        /// and opens dialog of <c cref="RemindersForm">reminder config form</c>.
        /// </summary>
        private void newReminder_button_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Remindes.json";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string reminderFile = saveFileDialog1.FileName;
                RemindersForm form = new(reminderFile);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// Loads reminders from existing file 
        /// and opens dialog of <c cref="RemindersForm">reminder config form</c>.
        /// </summary>
        private void existingRemider_button_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "Remindes.json";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string reminderFile = openFileDialog1.FileName;
                Instances.LoadReminders(reminderFile);
                RemindersForm form = new(reminderFile);
                form.ShowDialog();
            }
        }

        /// <summary>
        /// React to value change in the reminder days number box.
        /// </summary>
        private void reminderDays_number_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreReminderTime) return;
            if (selectedTask is null) return;
            UpdateReminderTime();
        }

        /// <summary>
        /// If set to true, the reminder time controls dont react on value changed event.
        /// </summary>
        private bool ignoreReminderTime = false;

        /// <summary>
        /// Updates the reminder time for the selected task, if the time is too long it also
        /// adjusts the controls values.
        /// </summary>
        private void UpdateReminderTime()
        {
            DateTime d = dateTimePicker2.Value;
            int days = (int)reminderDays_number.Value;
            TimeSpan newTime = new TimeSpan(days, d.Hour, d.Minute, d.Second);
            TimeSpan maxTime = selectedTask.GetMaxRemindSpan();
            TimeSpan minTime = new TimeSpan(0, 1, 0);
            if (newTime > maxTime)
            {
                ignoreReminderTime = true;
                dateTimePicker2.Value = new DateTime(d.Year, d.Month, d.Day, maxTime.Hours, maxTime.Minutes, maxTime.Seconds);
                reminderDays_number.Value = maxTime.Days;
                ignoreReminderTime = false;
                selectedTask.RemindSpan = maxTime;
            }
            else if (newTime < minTime)
            {
                ignoreReminderTime = true;
                dateTimePicker2.Value = new DateTime(d.Year, d.Month, d.Day, minTime.Hours, minTime.Minutes, minTime.Seconds);
                reminderDays_number.Value = minTime.Days;
                ignoreReminderTime = false;
                selectedTask.RemindSpan = minTime;
            }
            else
            {
                selectedTask.RemindSpan = newTime;
            }
        }
    }
}