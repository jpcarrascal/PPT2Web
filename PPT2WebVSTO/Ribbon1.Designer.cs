namespace PPT2WebVSTO
{
    partial class Ribbon1 : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon1()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon1));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.group1 = this.Factory.CreateRibbonGroup();
            this.checkBox1 = this.Factory.CreateRibbonCheckBox();
            this.URLbox = this.Factory.CreateRibbonEditBox();
            this.separator1 = this.Factory.CreateRibbonSeparator();
            this.separator2 = this.Factory.CreateRibbonSeparator();
            this.PPT2Web = this.Factory.CreateRibbonButton();
            this.CopyToClipboard = this.Factory.CreateRibbonButton();
            this.OpenInBrowser = this.Factory.CreateRibbonButton();
            this.deleteWebDeck = this.Factory.CreateRibbonButton();
            this.Settings = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.group1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.Groups.Add(this.group1);
            this.tab1.Label = "PP2Web";
            this.tab1.Name = "tab1";
            // 
            // group1
            // 
            this.group1.Items.Add(this.checkBox1);
            this.group1.Items.Add(this.URLbox);
            this.group1.Items.Add(this.PPT2Web);
            this.group1.Items.Add(this.separator1);
            this.group1.Items.Add(this.CopyToClipboard);
            this.group1.Items.Add(this.OpenInBrowser);
            this.group1.Items.Add(this.deleteWebDeck);
            this.group1.Items.Add(this.separator2);
            this.group1.Items.Add(this.Settings);
            this.group1.Label = "PPT to Web";
            this.group1.Name = "group1";
            // 
            // checkBox1
            // 
            this.checkBox1.Checked = true;
            this.checkBox1.Label = "Include hidden slides";
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.checkBox1_Click);
            // 
            // URLbox
            // 
            this.URLbox.Enabled = false;
            this.URLbox.Label = "Loc:";
            this.URLbox.Name = "URLbox";
            this.URLbox.SizeString = "https://ppt2webfrontend";
            this.URLbox.Text = "<deck not exported>";
            this.URLbox.TextChanged += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.URLbox_TextChanged);
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            // 
            // PPT2Web
            // 
            this.PPT2Web.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PPT2Web.Enabled = false;
            this.PPT2Web.Image = ((System.Drawing.Image)(resources.GetObject("PPT2Web.Image")));
            this.PPT2Web.Label = "Export to Web";
            this.PPT2Web.Name = "PPT2Web";
            this.PPT2Web.ShowImage = true;
            this.PPT2Web.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Export2Web_Click);
            // 
            // CopyToClipboard
            // 
            this.CopyToClipboard.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CopyToClipboard.Enabled = false;
            this.CopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("CopyToClipboard.Image")));
            this.CopyToClipboard.Label = "Copy URL to clipboard";
            this.CopyToClipboard.Name = "CopyToClipboard";
            this.CopyToClipboard.ShowImage = true;
            this.CopyToClipboard.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CopyToClipboard_Click);
            // 
            // OpenInBrowser
            // 
            this.OpenInBrowser.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.OpenInBrowser.Enabled = false;
            this.OpenInBrowser.Image = global::PPT2WebVSTO.Properties.Resources.browser;
            this.OpenInBrowser.Label = "Open URL in browser";
            this.OpenInBrowser.Name = "OpenInBrowser";
            this.OpenInBrowser.ShowImage = true;
            this.OpenInBrowser.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenInBrowser_Click);
            // 
            // deleteWebDeck
            // 
            this.deleteWebDeck.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.deleteWebDeck.Image = global::PPT2WebVSTO.Properties.Resources.delete;
            this.deleteWebDeck.Label = "Delete from Web";
            this.deleteWebDeck.Name = "deleteWebDeck";
            this.deleteWebDeck.ShowImage = true;
            this.deleteWebDeck.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.deleteWebDeck_Click);
            // 
            // Settings
            // 
            this.Settings.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.Settings.Image = global::PPT2WebVSTO.Properties.Resources.settings;
            this.Settings.Label = "Settings";
            this.Settings.Name = "Settings";
            this.Settings.ShowImage = true;
            // 
            // Ribbon1
            // 
            this.Name = "Ribbon1";
            this.RibbonType = "Microsoft.PowerPoint.Presentation";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.Ribbon1_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup group1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PPT2Web;
        internal Microsoft.Office.Tools.Ribbon.RibbonCheckBox checkBox1;
        internal Microsoft.Office.Tools.Ribbon.RibbonEditBox URLbox;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CopyToClipboard;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator1;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenInBrowser;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton deleteWebDeck;
        internal Microsoft.Office.Tools.Ribbon.RibbonSeparator separator2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Settings;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon1 Ribbon1
        {
            get { return this.GetRibbon<Ribbon1>(); }
        }
    }
}
