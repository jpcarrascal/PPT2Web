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
        public SettingsDialog(string url)
        {
            InitializeComponent();
            saveSettings.Enabled = false;
            serverURL.Text = url;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var uriName = serverURL.Text;
            Uri uriResult;
            bool isURL = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isURL)
            {
                wrongURL.Visible = false;
                saveSettings.Enabled = false;
                Properties.Settings.Default.uploadURL = serverURL.Text;
                this.Close();
            }
            else
            {
                wrongURL.Visible = true;
            }

        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            serverURL.Text = Properties.Settings.Default.uploadURL;
            wrongURL.Visible = false;
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void serverURL_TextChanged(object sender, EventArgs e)
        {
            var uriName = serverURL.Text;
            Uri uriResult;
            bool isURL = Uri.TryCreate(uriName, UriKind.Absolute, out uriResult)
                            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isURL)
            {
                wrongURL.Visible = false;
                saveSettings.Enabled = true;
            }
            else
            {
                saveSettings.Enabled = false;
                wrongURL.Visible = true;
            }
        }

        private void wrongURL_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
