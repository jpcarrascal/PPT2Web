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
            this.uploadURL = new System.Windows.Forms.TextBox();
            this.uploadLabel = new System.Windows.Forms.Label();
            this.wrongServerURL = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.showLabel = new System.Windows.Forms.Label();
            this.showURL = new System.Windows.Forms.TextBox();
            this.wrongShowURL = new System.Windows.Forms.Label();
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
            // uploadURL
            // 
            resources.ApplyResources(this.uploadURL, "uploadURL");
            this.uploadURL.Name = "uploadURL";
            this.uploadURL.TextChanged += new System.EventHandler(this.serverURL_TextChanged);
            // 
            // uploadLabel
            // 
            resources.ApplyResources(this.uploadLabel, "uploadLabel");
            this.uploadLabel.Name = "uploadLabel";
            // 
            // wrongServerURL
            // 
            resources.ApplyResources(this.wrongServerURL, "wrongServerURL");
            this.wrongServerURL.ForeColor = System.Drawing.Color.Red;
            this.wrongServerURL.Name = "wrongServerURL";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // showLabel
            // 
            resources.ApplyResources(this.showLabel, "showLabel");
            this.showLabel.Name = "showLabel";
            // 
            // showURL
            // 
            resources.ApplyResources(this.showURL, "showURL");
            this.showURL.Name = "showURL";
            this.showURL.TextChanged += new System.EventHandler(this.showURL_TextChanged);
            // 
            // wrongShowURL
            // 
            resources.ApplyResources(this.wrongShowURL, "wrongShowURL");
            this.wrongShowURL.ForeColor = System.Drawing.Color.Red;
            this.wrongShowURL.Name = "wrongShowURL";
            // 
            // SettingsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.wrongShowURL);
            this.Controls.Add(this.showURL);
            this.Controls.Add(this.showLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wrongServerURL);
            this.Controls.Add(this.uploadLabel);
            this.Controls.Add(this.uploadURL);
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
        private System.Windows.Forms.TextBox uploadURL;
        private System.Windows.Forms.Label uploadLabel;
        private System.Windows.Forms.Label wrongServerURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label showLabel;
        private System.Windows.Forms.TextBox showURL;
        private System.Windows.Forms.Label wrongShowURL;
    }
}