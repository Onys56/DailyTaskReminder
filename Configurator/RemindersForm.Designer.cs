
namespace Configurator
{
    partial class RemindersForm
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
            this.reminderTypes_listBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.addReminder_button = new System.Windows.Forms.Button();
            this.reminder_listBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.fields_listBox = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.deleteReminder_button = new System.Windows.Forms.Button();
            this.field_textBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.save_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // reminderTypes_listBox
            // 
            this.reminderTypes_listBox.FormattingEnabled = true;
            this.reminderTypes_listBox.ItemHeight = 15;
            this.reminderTypes_listBox.Location = new System.Drawing.Point(145, 62);
            this.reminderTypes_listBox.Name = "reminderTypes_listBox";
            this.reminderTypes_listBox.Size = new System.Drawing.Size(146, 79);
            this.reminderTypes_listBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(145, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 46);
            this.label1.TabIndex = 1;
            this.label1.Text = "To add a reminder select its type:";
            // 
            // addReminder_button
            // 
            this.addReminder_button.Location = new System.Drawing.Point(145, 147);
            this.addReminder_button.Name = "addReminder_button";
            this.addReminder_button.Size = new System.Drawing.Size(146, 34);
            this.addReminder_button.TabIndex = 2;
            this.addReminder_button.Text = "Add reminder";
            this.addReminder_button.UseVisualStyleBackColor = true;
            this.addReminder_button.Click += new System.EventHandler(this.addReminder_button_Click);
            // 
            // reminder_listBox
            // 
            this.reminder_listBox.FormattingEnabled = true;
            this.reminder_listBox.ItemHeight = 15;
            this.reminder_listBox.Location = new System.Drawing.Point(13, 36);
            this.reminder_listBox.Name = "reminder_listBox";
            this.reminder_listBox.Size = new System.Drawing.Size(127, 334);
            this.reminder_listBox.TabIndex = 3;
            this.reminder_listBox.SelectedIndexChanged += new System.EventHandler(this.reminder_listBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Current reminders";
            // 
            // fields_listBox
            // 
            this.fields_listBox.FormattingEnabled = true;
            this.fields_listBox.ItemHeight = 15;
            this.fields_listBox.Location = new System.Drawing.Point(145, 246);
            this.fields_listBox.Name = "fields_listBox";
            this.fields_listBox.Size = new System.Drawing.Size(146, 124);
            this.fields_listBox.TabIndex = 5;
            this.fields_listBox.SelectedIndexChanged += new System.EventHandler(this.fields_listBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(146, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Reminder fields";
            // 
            // deleteReminder_button
            // 
            this.deleteReminder_button.Location = new System.Drawing.Point(145, 187);
            this.deleteReminder_button.Name = "deleteReminder_button";
            this.deleteReminder_button.Size = new System.Drawing.Size(146, 34);
            this.deleteReminder_button.TabIndex = 7;
            this.deleteReminder_button.Text = "Delete selected reminder";
            this.deleteReminder_button.UseVisualStyleBackColor = true;
            this.deleteReminder_button.Click += new System.EventHandler(this.deleteReminder_button_Click);
            // 
            // field_textBox
            // 
            this.field_textBox.Location = new System.Drawing.Point(12, 405);
            this.field_textBox.Name = "field_textBox";
            this.field_textBox.Size = new System.Drawing.Size(279, 23);
            this.field_textBox.TabIndex = 8;
            this.field_textBox.TextChanged += new System.EventHandler(this.field_textBox_TextChanged);
            this.field_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.field_textBox_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(12, 382);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(220, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Value of currently selected field:";
            // 
            // save_button
            // 
            this.save_button.Location = new System.Drawing.Point(13, 435);
            this.save_button.Name = "save_button";
            this.save_button.Size = new System.Drawing.Size(127, 39);
            this.save_button.TabIndex = 10;
            this.save_button.Text = "Save reminders to file";
            this.save_button.UseVisualStyleBackColor = true;
            this.save_button.Click += new System.EventHandler(this.save_button_Click);
            // 
            // cancel_button
            // 
            this.cancel_button.Location = new System.Drawing.Point(147, 435);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(144, 39);
            this.cancel_button.TabIndex = 11;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            this.cancel_button.Click += new System.EventHandler(this.cancel_button_Click);
            // 
            // RemindersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 482);
            this.Controls.Add(this.cancel_button);
            this.Controls.Add(this.save_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.field_textBox);
            this.Controls.Add(this.deleteReminder_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fields_listBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reminder_listBox);
            this.Controls.Add(this.addReminder_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reminderTypes_listBox);
            this.Name = "RemindersForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Reminders  configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox reminderTypes_listBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button addReminder_button;
        private System.Windows.Forms.ListBox reminder_listBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox fields_listBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button deleteReminder_button;
        private System.Windows.Forms.TextBox field_textBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button save_button;
        private System.Windows.Forms.Button cancel_button;
    }
}