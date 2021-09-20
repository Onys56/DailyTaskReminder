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
    public partial class NewTaskDialog : Form
    {
        private MainForm mainForm;
        public NewTaskDialog(MainForm main)
        {
            InitializeComponent();
            type_dropDown.SelectedIndex = 0;
            mainForm = main;
        }

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
            mainForm.AddTask(t);
            Close();
        }

        private void cancel_button_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
