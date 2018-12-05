using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPT2WebVSTO
{
    public partial class SettingsDialog : Form
    {
        public SettingsDialog(string upURL, string shURL)
        {
            InitializeComponent();
            saveSettings.Enabled = false;
            uploadURL.Text = upURL;
            showURL.Text = shURL;
        }

        private bool serverChanged = false;
        private bool showChanged = false;

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (serverChanged)
                Properties.Settings.Default.uploadURL = uploadURL.Text;
            if (showChanged)
                Properties.Settings.Default.showURL = showURL.Text;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            uploadURL.Text = Properties.Settings.Default.uploadURL;
            showURL.Text = Properties.Settings.Default.showURL;
            wrongServerURL.Visible = false;
            wrongShowURL.Visible = false;
            this.Close();
        }

        private void serverURL_TextChanged(object sender, EventArgs e)
        {
            var uriName = uploadURL.Text;
            Uri uriResult;
            bool isURL = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isURL)
            {
                wrongServerURL.Visible = false;
                serverChanged = true;
                if (!wrongShowURL.Visible)
                    saveSettings.Enabled = true;
            }
            else
            {
                saveSettings.Enabled = false;
                wrongServerURL.Visible = true;
            }
        }

        private void showURL_TextChanged(object sender, EventArgs e)
        {
            var uriName = showURL.Text;
            Uri uriResult;
            bool isURL = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isURL)
            {
                wrongShowURL.Visible = false;
                showChanged = true;
                if(!wrongServerURL.Visible)
                    saveSettings.Enabled = true;
            }
            else
            {
                saveSettings.Enabled = false;
                wrongShowURL.Visible = true;
            }
        }
    }
}
