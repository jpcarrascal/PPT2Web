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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
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
            resources.ApplyResources(this.cancelSettings, "cancelSettings");
            this.cancelSettings.Name = "cancelSettings";
            this.cancelSettings.UseVisualStyleBackColor = true;
            this.cancelSettings.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // saveSettings
            // 
            resources.ApplyResources(this.saveSettings, "saveSettings");
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serverURL
            // 
            resources.ApplyResources(this.serverURL, "serverURL");
            this.serverURL.Name = "serverURL";
            this.serverURL.TextChanged += new System.EventHandler(this.serverURL_TextChanged);
            // 
            // serverLabel
            // 
            resources.ApplyResources(this.serverLabel, "serverLabel");
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // wrongURL
            // 
            resources.ApplyResources(this.wrongURL, "wrongURL");
            this.wrongURL.ForeColor = System.Drawing.Color.Red;
            this.wrongURL.Name = "wrongURL";
            this.wrongURL.Click += new System.EventHandler(this.wrongURL_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // SettingsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wrongURL);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.serverURL);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.cancelSettings);
            this.Name = "SettingsDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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