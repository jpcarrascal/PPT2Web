namespace PPT2WebVSTO
{
    partial class SettingsDialog
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
            this.cancelSettings = new System.Windows.Forms.Button();
            this.saveSettings = new System.Windows.Forms.Button();
            this.serverURL = new System.Windows.Forms.TextBox();
            this.serverLabel = new System.Windows.Forms.Label();
            this.wrongURL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cancelSettings
            // 
            this.cancelSettings.Location = new System.Drawing.Point(345, 78);
            this.cancelSettings.Name = "cancelSettings";
            this.cancelSettings.Size = new System.Drawing.Size(75, 23);
            this.cancelSettings.TabIndex = 0;
            this.cancelSettings.Text = "Cancel";
            this.cancelSettings.UseVisualStyleBackColor = true;
            this.cancelSettings.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveSettings
            // 
            this.saveSettings.Enabled = false;
            this.saveSettings.Location = new System.Drawing.Point(252, 78);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(75, 23);
            this.saveSettings.TabIndex = 1;
            this.saveSettings.Text = "Save";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serverURL
            // 
            this.serverURL.Location = new System.Drawing.Point(82, 37);
            this.serverURL.Name = "serverURL";
            this.serverURL.Size = new System.Drawing.Size(338, 20);
            this.serverURL.TabIndex = 2;
            this.serverURL.TextChanged += new System.EventHandler(this.serverURL_TextChanged);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(13, 40);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(66, 13);
            this.serverLabel.TabIndex = 3;
            this.serverLabel.Text = "Server URL:";
            this.serverLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // wrongURL
            // 
            this.wrongURL.AutoSize = true;
            this.wrongURL.ForeColor = System.Drawing.Color.Red;
            this.wrongURL.Location = new System.Drawing.Point(79, 60);
            this.wrongURL.Name = "wrongURL";
            this.wrongURL.Size = new System.Drawing.Size(109, 13);
            this.wrongURL.TabIndex = 4;
            this.wrongURL.Text = "Incorrect URL format!";
            this.wrongURL.Visible = false;
            this.wrongURL.Click += new System.EventHandler(this.wrongURL_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(292, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Warning! Changing this setting might make break this add-in.";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // SettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 113);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wrongURL);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.serverURL);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.cancelSettings);
            this.Name = "SettingsDialog";
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelSettings;
        private System.Windows.Forms.Button saveSettings;
        private System.Windows.Forms.TextBox serverURL;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label wrongURL;
        private System.Windows.Forms.Label label1;
    }
}