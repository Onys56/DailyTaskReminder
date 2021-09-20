using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DailyTaskReminder.Tasks;

namespace Configurator
{
    /// <summary>
    /// Form for adding new tasks.
    /// Letting the user choose the type of the task (such as Daily, Weekly, etc.)
    /// </summary>
    public partial class NewTaskDialog : Form
    {
        private MainForm mainForm;
        public NewTaskDialog(MainForm main)
        {
            InitializeComponent();
            type_dropDown.SelectedIndex = 0;
            mainForm = main;
        }

        /// <summary>
        /// Confirms the selection of task type;
        /// </summary>
        private void select_button_Click(object sender, EventArgs e)
        {
            Task t = type_dropDown.SelectedItem switch
            {
                "Daily" => new DailyTask(),
                "Weekly" => new WeeklyTask(),
                "Monthly" => new MonthlyTask(),
                "Yearly" => new YearlyTask(),
                _ => throw new Exception("Unknown task type")
            };
            t.Name = $"NewTask {mainForm.Tasks.Count}";
            t.RemindSpan = new TimeSpan(1, 0, 0);
            mainForm.AddTask(t);
            Close();
        }

        /// <summary>
        /// Handles the cancelation of adding the task.
        /// </summary>
        private void cancel_button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
