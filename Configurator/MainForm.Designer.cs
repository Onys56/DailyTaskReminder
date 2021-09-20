﻿
namespace Configurator
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button_existing_file = new System.Windows.Forms.Button();
            this.button_new_file = new System.Windows.Forms.Button();
            this.TaskList = new System.Windows.Forms.ListBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button_back_to_menu = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.taskList_label = new System.Windows.Forms.Label();
            this.dueTime_label = new System.Windows.Forms.Label();
            this.taskName_textBox = new System.Windows.Forms.TextBox();
            this.taskName_label = new System.Windows.Forms.Label();
            this.remindTime_label = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.reminders_label = new System.Windows.Forms.Label();
            this.reminders_listBox = new System.Windows.Forms.ListBox();
            this.remindersAdd_button = new System.Windows.Forms.Button();
            this.remindersDelete_button = new System.Windows.Forms.Button();
            this.weekDays_label = new System.Windows.Forms.Label();
            this.weekDays_checkBox = new System.Windows.Forms.CheckedListBox();
            this.monthDay_label = new System.Windows.Forms.Label();
            this.month_label = new System.Windows.Forms.Label();
            this.day_number = new System.Windows.Forms.NumericUpDown();
            this.month_number = new System.Windows.Forms.NumericUpDown();
            this.addTask_button = new System.Windows.Forms.Button();
            this.saveToFile_button = new System.Windows.Forms.Button();
            this.deleteTask_Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.day_number)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.month_number)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "tasks.txt";
            // 
            // button_existing_file
            // 
            this.button_existing_file.Location = new System.Drawing.Point(307, 157);
            this.button_existing_file.Name = "button_existing_file";
            this.button_existing_file.Size = new System.Drawing.Size(178, 35);
            this.button_existing_file.TabIndex = 1;
            this.button_existing_file.Text = "Open existing config file";
            this.button_existing_file.UseVisualStyleBackColor = true;
            this.button_existing_file.Click += new System.EventHandler(this.button_existing_file_Click);
            // 
            // button_new_file
            // 
            this.button_new_file.Location = new System.Drawing.Point(307, 92);
            this.button_new_file.Name = "button_new_file";
            this.button_new_file.Size = new System.Drawing.Size(178, 35);
            this.button_new_file.TabIndex = 2;
            this.button_new_file.Text = "Create new config file";
            this.button_new_file.UseVisualStyleBackColor = true;
            this.button_new_file.Click += new System.EventHandler(this.button_new_file_Click);
            // 
            // TaskList
            // 
            this.TaskList.FormattingEnabled = true;
            this.TaskList.ItemHeight = 15;
            this.TaskList.Location = new System.Drawing.Point(581, 71);
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(161, 259);
            this.TaskList.TabIndex = 3;
            this.TaskList.SelectedIndexChanged += new System.EventHandler(this.TaskList_SelectedIndexChanged);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "tasks.txt";
            this.saveFileDialog1.Title = "Choose where to save the config file";
            // 
            // button_back_to_menu
            // 
            this.button_back_to_menu.Location = new System.Drawing.Point(12, 12);
            this.button_back_to_menu.Name = "button_back_to_menu";
            this.button_back_to_menu.Size = new System.Drawing.Size(92, 29);
            this.button_back_to_menu.TabIndex = 4;
            this.button_back_to_menu.Text = "Back to menu";
            this.button_back_to_menu.UseVisualStyleBackColor = true;
            this.button_back_to_menu.Click += new System.EventHandler(this.button_back_to_menu_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker1.Location = new System.Drawing.Point(267, 134);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(80, 23);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.Value = new System.DateTime(1753, 1, 1, 12, 0, 0, 0);
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(237, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(331, 30);
            this.label1.TabIndex = 6;
            this.label1.Text = "Daily Task Reminder configurator";
            // 
            // taskList_label
            // 
            this.taskList_label.AutoSize = true;
            this.taskList_label.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.taskList_label.Location = new System.Drawing.Point(581, 47);
            this.taskList_label.Name = "taskList_label";
            this.taskList_label.Size = new System.Drawing.Size(102, 21);
            this.taskList_label.TabIndex = 7;
            this.taskList_label.Text = "Current tasks";
            // 
            // dueTime_label
            // 
            this.dueTime_label.AutoSize = true;
            this.dueTime_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.dueTime_label.Location = new System.Drawing.Point(107, 136);
            this.dueTime_label.Name = "dueTime_label";
            this.dueTime_label.Size = new System.Drawing.Size(103, 20);
            this.dueTime_label.TabIndex = 8;
            this.dueTime_label.Text = "Deadline time";
            // 
            // taskName_textBox
            // 
            this.taskName_textBox.Location = new System.Drawing.Point(267, 104);
            this.taskName_textBox.Name = "taskName_textBox";
            this.taskName_textBox.Size = new System.Drawing.Size(139, 23);
            this.taskName_textBox.TabIndex = 0;
            this.taskName_textBox.TextChanged += new System.EventHandler(this.taskName_textBox_TextChanged);
            this.taskName_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.taskName_textBox_KeyDown);
            // 
            // taskName_label
            // 
            this.taskName_label.AutoSize = true;
            this.taskName_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.taskName_label.Location = new System.Drawing.Point(107, 105);
            this.taskName_label.Name = "taskName_label";
            this.taskName_label.Size = new System.Drawing.Size(77, 20);
            this.taskName_label.TabIndex = 10;
            this.taskName_label.Text = "Task name";
            // 
            // remindTime_label
            // 
            this.remindTime_label.AutoSize = true;
            this.remindTime_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.remindTime_label.Location = new System.Drawing.Point(107, 171);
            this.remindTime_label.Name = "remindTime_label";
            this.remindTime_label.Size = new System.Drawing.Size(154, 20);
            this.remindTime_label.TabIndex = 11;
            this.remindTime_label.Text = "Time before reminder";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePicker2.Location = new System.Drawing.Point(267, 169);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.ShowUpDown = true;
            this.dateTimePicker2.Size = new System.Drawing.Size(80, 23);
            this.dateTimePicker2.TabIndex = 2;
            this.dateTimePicker2.Value = new System.DateTime(2021, 9, 20, 1, 0, 0, 0);
            this.dateTimePicker2.ValueChanged += new System.EventHandler(this.dateTimePicker2_ValueChanged);
            // 
            // reminders_label
            // 
            this.reminders_label.AutoSize = true;
            this.reminders_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.reminders_label.Location = new System.Drawing.Point(107, 205);
            this.reminders_label.Name = "reminders_label";
            this.reminders_label.Size = new System.Drawing.Size(79, 20);
            this.reminders_label.TabIndex = 13;
            this.reminders_label.Text = "Reminders";
            // 
            // reminders_listBox
            // 
            this.reminders_listBox.FormattingEnabled = true;
            this.reminders_listBox.ItemHeight = 15;
            this.reminders_listBox.Location = new System.Drawing.Point(267, 208);
            this.reminders_listBox.Name = "reminders_listBox";
            this.reminders_listBox.Size = new System.Drawing.Size(158, 49);
            this.reminders_listBox.TabIndex = 3;
            // 
            // remindersAdd_button
            // 
            this.remindersAdd_button.Location = new System.Drawing.Point(443, 205);
            this.remindersAdd_button.Name = "remindersAdd_button";
            this.remindersAdd_button.Size = new System.Drawing.Size(75, 23);
            this.remindersAdd_button.TabIndex = 15;
            this.remindersAdd_button.Text = "Add";
            this.remindersAdd_button.UseVisualStyleBackColor = true;
            this.remindersAdd_button.Click += new System.EventHandler(this.remindersAdd_button_Click);
            // 
            // remindersDelete_button
            // 
            this.remindersDelete_button.Location = new System.Drawing.Point(443, 234);
            this.remindersDelete_button.Name = "remindersDelete_button";
            this.remindersDelete_button.Size = new System.Drawing.Size(75, 23);
            this.remindersDelete_button.TabIndex = 16;
            this.remindersDelete_button.Text = "Delete";
            this.remindersDelete_button.UseVisualStyleBackColor = true;
            this.remindersDelete_button.Click += new System.EventHandler(this.remindersDelete_button_Click);
            // 
            // weekDays_label
            // 
            this.weekDays_label.AutoSize = true;
            this.weekDays_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.weekDays_label.Location = new System.Drawing.Point(107, 268);
            this.weekDays_label.Name = "weekDays_label";
            this.weekDays_label.Size = new System.Drawing.Size(79, 20);
            this.weekDays_label.TabIndex = 17;
            this.weekDays_label.Text = "Week days";
            // 
            // weekDays_checkBox
            // 
            this.weekDays_checkBox.CheckOnClick = true;
            this.weekDays_checkBox.FormattingEnabled = true;
            this.weekDays_checkBox.Items.AddRange(new object[] {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"});
            this.weekDays_checkBox.Location = new System.Drawing.Point(267, 268);
            this.weekDays_checkBox.Name = "weekDays_checkBox";
            this.weekDays_checkBox.Size = new System.Drawing.Size(158, 130);
            this.weekDays_checkBox.TabIndex = 18;
            this.weekDays_checkBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.weekDays_checkBox_ItemCheck);
            // 
            // monthDay_label
            // 
            this.monthDay_label.AutoSize = true;
            this.monthDay_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.monthDay_label.Location = new System.Drawing.Point(107, 273);
            this.monthDay_label.Name = "monthDay_label";
            this.monthDay_label.Size = new System.Drawing.Size(125, 20);
            this.monthDay_label.TabIndex = 19;
            this.monthDay_label.Text = "Day of the month";
            // 
            // month_label
            // 
            this.month_label.AutoSize = true;
            this.month_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.month_label.Location = new System.Drawing.Point(107, 299);
            this.month_label.Name = "month_label";
            this.month_label.Size = new System.Drawing.Size(52, 20);
            this.month_label.TabIndex = 20;
            this.month_label.Text = "Month";
            // 
            // day_number
            // 
            this.day_number.Location = new System.Drawing.Point(267, 270);
            this.day_number.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.day_number.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.day_number.Name = "day_number";
            this.day_number.Size = new System.Drawing.Size(120, 23);
            this.day_number.TabIndex = 4;
            this.day_number.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.day_number.ValueChanged += new System.EventHandler(this.day_number_ValueChanged);
            // 
            // month_number
            // 
            this.month_number.Location = new System.Drawing.Point(267, 299);
            this.month_number.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.month_number.Name = "month_number";
            this.month_number.Size = new System.Drawing.Size(120, 23);
            this.month_number.TabIndex = 5;
            this.month_number.ValueChanged += new System.EventHandler(this.month_number_ValueChanged);
            // 
            // addTask_button
            // 
            this.addTask_button.Location = new System.Drawing.Point(581, 336);
            this.addTask_button.Name = "addTask_button";
            this.addTask_button.Size = new System.Drawing.Size(161, 24);
            this.addTask_button.TabIndex = 23;
            this.addTask_button.Text = "Add new task";
            this.addTask_button.UseVisualStyleBackColor = true;
            this.addTask_button.Click += new System.EventHandler(this.addTask_button_Click);
            // 
            // saveToFile_button
            // 
            this.saveToFile_button.Location = new System.Drawing.Point(581, 397);
            this.saveToFile_button.Name = "saveToFile_button";
            this.saveToFile_button.Size = new System.Drawing.Size(161, 41);
            this.saveToFile_button.TabIndex = 24;
            this.saveToFile_button.Text = "Save to file";
            this.saveToFile_button.UseVisualStyleBackColor = true;
            this.saveToFile_button.Click += new System.EventHandler(this.saveToFile_button_Click);
            // 
            // deleteTask_Button
            // 
            this.deleteTask_Button.Location = new System.Drawing.Point(581, 366);
            this.deleteTask_Button.Name = "deleteTask_Button";
            this.deleteTask_Button.Size = new System.Drawing.Size(161, 25);
            this.deleteTask_Button.TabIndex = 25;
            this.deleteTask_Button.Text = "Delete selected task";
            this.deleteTask_Button.Click += new System.EventHandler(this.deleteTask_Button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.deleteTask_Button);
            this.Controls.Add(this.saveToFile_button);
            this.Controls.Add(this.addTask_button);
            this.Controls.Add(this.month_number);
            this.Controls.Add(this.day_number);
            this.Controls.Add(this.month_label);
            this.Controls.Add(this.monthDay_label);
            this.Controls.Add(this.remindersDelete_button);
            this.Controls.Add(this.remindersAdd_button);
            this.Controls.Add(this.reminders_listBox);
            this.Controls.Add(this.reminders_label);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.remindTime_label);
            this.Controls.Add(this.taskName_label);
            this.Controls.Add(this.taskName_textBox);
            this.Controls.Add(this.dueTime_label);
            this.Controls.Add(this.taskList_label);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.button_back_to_menu);
            this.Controls.Add(this.TaskList);
            this.Controls.Add(this.button_new_file);
            this.Controls.Add(this.button_existing_file);
            this.Controls.Add(this.weekDays_checkBox);
            this.Controls.Add(this.weekDays_label);
            this.Name = "MainForm";
            this.Text = "Daily Task Reminder configurator";
            ((System.ComponentModel.ISupportInitialize)(this.day_number)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.month_number)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_existing_file;
        private System.Windows.Forms.Button button_new_file;
        private System.Windows.Forms.ListBox TaskList;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button_back_to_menu;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label taskList_label;
        private System.Windows.Forms.Label dueTime_label;
        private System.Windows.Forms.TextBox taskName_textBox;
        private System.Windows.Forms.Label taskName_label;
        private System.Windows.Forms.Label remindTime_label;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label reminders_label;
        private System.Windows.Forms.ListBox reminders_listBox;
        private System.Windows.Forms.Button remindersAdd_button;
        private System.Windows.Forms.Button remindersDelete_button;
        private System.Windows.Forms.Label weekDays_label;
        private System.Windows.Forms.CheckedListBox weekDays_checkBox;
        private System.Windows.Forms.Label monthDay_label;
        private System.Windows.Forms.Label month_label;
        private System.Windows.Forms.NumericUpDown day_number;
        private System.Windows.Forms.NumericUpDown month_number;
        private System.Windows.Forms.Button addTask_button;
        private System.Windows.Forms.Button saveToFile_button;
        private System.Windows.Forms.Button deleteTask_Button;
    }
}

