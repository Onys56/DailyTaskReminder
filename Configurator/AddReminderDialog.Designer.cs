
namespace Configurator
{
    partial class AddReminderDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadReminders_button = new System.Windows.Forms.Button();
            this.reminderStatus = new System.Windows.Forms.Label();
            this.reminderStatus_label = new System.Windows.Forms.Label();
            this.add_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.reminder_textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // loadReminders_button
            // 
            this.loadReminders_button.Location = new System.Drawing.Point(327, 62);
            this.loadReminders_button.Name = "loadReminders_button";
            this.loadReminders_button.Size = new System.Drawing.Size(66, 23);
            this.loadReminders_button.TabIndex = 35;
            this.loadReminders_button.Text = "Load";
            this.loadReminders_button.UseVisualStyleBackColor = true;
            this.loadReminders_button.Click += new System.EventHandler(this.loadReminders_button_Click);
            // 
            // reminderStatus
            // 
            this.reminderStatus.AutoSize = true;
            this.reminderStatus.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.reminderStatus.Location = new System.Drawing.Point(305, 39);
            this.reminderStatus.Name = "reminderStatus";
            this.reminderStatus.Size = new System.Drawing.Size(88, 20);
            this.reminderStatus.TabIndex = 34;
            this.reminderStatus.Text = "Not Loaded";
            // 
            // reminderStatus_label
            // 
            this.reminderStatus_label.AutoSize = true;
            this.reminderStatus_label.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.reminderStatus_label.Location = new System.Drawing.Point(175, 39);
            this.reminderStatus_label.Name = "reminderStatus_label";
            this.reminderStatus_label.Size = new System.Drawing.Size(124, 20);
            this.reminderStatus_label.TabIndex = 33;
            this.reminderStatus_label.Text = "Reminders status:";
            // 
            // add_button
            // 
            this.add_button.Location = new System.Drawing.Point(11, 91);
            this.add_button.Name = "add_button";
            this.add_button.Size = new System.Drawing.Size(94, 37);
            this.add_button.TabIndex = 36;
            this.add_button.Text = "Add";
            this.add_button.UseVisualStyleBackColor = true;
            this.add_button.Click += new System.EventHandler(this.add_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(288, 91);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(105, 37);
            this.cancel_button.TabIndex = 37;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // reminder_textBox
            // 
            this.reminder_textBox.Location = new System.Drawing.Point(12, 59);
            this.reminder_textBox.Name = "reminder_textBox";
            this.reminder_textBox.Size = new System.Drawing.Size(185, 23);
            this.reminder_textBox.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 50);
            this.label1.TabIndex = 39;
            this.label1.Text = "Type in the name of the reminder (the one specified in the reminder config file)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(175, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(218, 33);
            this.label2.TabIndex = 40;
            this.label2.Text = "You can also load reminders from a file so that the name autocompletes";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "Reminders.json";
            this.openFileDialog1.Title = "Open reminders config file";
            // 
            // AddReminderDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 136);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reminder_textBox);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.add_button);
            this.Controls.Add(this.loadReminders_button);
            this.Controls.Add(this.reminderStatus);
            this.Controls.Add(this.reminderStatus_label);
            this.Name = "AddReminderDialog";
            this.Text = "Add reminder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadReminders_button;
        private System.Windows.Forms.Label reminderStatus;
        private System.Windows.Forms.Label reminderStatus_label;
        private System.Windows.Forms.Button add_button;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.TextBox reminder_textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}