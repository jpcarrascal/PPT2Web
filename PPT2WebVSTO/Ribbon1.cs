using System;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Microsoft.Office.Interop.PowerPoint;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.IO;
using System.Diagnostics;
using Microsoft.Office.Core;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace PPT2WebVSTO
{
    /*
     HKEY_CURRENT_USER\Software\Microsoft\VSTA\Solutions
    */
    public partial class Ribbon1
    {
        private readonly string url = Properties.Settings.Default.uploadURL;
        SettingsDialog settingsDialog = new SettingsDialog(Properties.Settings.Default.uploadURL);
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            var app = Globals.ThisAddIn.Application;
            app.AfterPresentationOpen += AfterPresentationOpenHandle;
            app.AfterNewPresentation += AfterPresentationOpenHandle;
            app.PresentationBeforeClose += resetControls;
            app.WindowActivate += SwitchWindowsHandle;
        }

        private void SwitchWindowsHandle(Presentation pres, DocumentWindow window)
        {
            PPT2Web.Enabled = true;
            Presentation pptPresentation = window.Presentation;
            if (ReadDocumentProperty(pptPresentation, "PPT2Web dir") != null)
            {
                URLbox.Text = ReadDocumentProperty(pptPresentation, "PPT2Web dir");
                URLbox.Enabled = true;
                CopyToClipboard.Enabled = true;
                deleteFromWeb.Enabled = true;
                OpenInBrowser.Enabled = true;
            }
            else
            {
                URLbox.Text = "";
                URLbox.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
            }

        }

        private void AfterPresentationOpenHandle(Presentation pptPresentation)
        {
            PPT2Web.Enabled = true;
            if (ReadDocumentProperty(pptPresentation, "PPT2Web dir") != null)
            {
                URLbox.Text = ReadDocumentProperty(pptPresentation, "PPT2Web dir");
                URLbox.Enabled = true;
                CopyToClipboard.Enabled = true;
                deleteFromWeb.Enabled = true;
                OpenInBrowser.Enabled = true;
            }
            else
            {
                URLbox.Text = "";
                URLbox.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
            }
        }

        private void resetControls(Presentation pres, ref bool cancel)
        {
            URLbox.Text = "";
            URLbox.Enabled = false;
            CopyToClipboard.Enabled = false;
            deleteFromWeb.Enabled = false;
            OpenInBrowser.Enabled = false;
            PPT2Web.Enabled = false;
        }

        private void Export2Web_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Presentation pptPresentation = Globals.ThisAddIn.GetActiveDeck();
                URLbox.Text = "";
                URLbox.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
                PPT2Web.Enabled = false;

                string pptLocation = pptPresentation.FullName;
                int numSlides = pptPresentation.Slides.Count;
                Debug.Print("There are " + numSlides.ToString() + " slides, dude.");
                // Create a temporary folder:
                Guid uniqueID = Guid.NewGuid();
                string destinationPath = Path.Combine(Path.GetTempPath(),  Path.GetFileNameWithoutExtension(pptPresentation.FullName));
                string destinationPathTmp = destinationPath + uniqueID;
                Debug.Print(destinationPath);
                if (Directory.Exists(destinationPathTmp))
                {
                    DirectoryInfo dir = new DirectoryInfo(destinationPathTmp);
                    dir.Delete(true);
                }
                else
                    Directory.CreateDirectory(destinationPathTmp);
                var csv = new StringBuilder();
                foreach (Slide slide in pptPresentation.Slides)
                {
                    //if (slide.HasNotesPage == MsoTriState.msoTrue && (slide.SlideShowTransition.Hidden == MsoTriState.msoFalse || this.checkBox1.Checked == true))
                    if (slide.SlideShowTransition.Hidden == MsoTriState.msoFalse || this.checkBox1.Checked == true)
                    {
                        // From: https://stackoverflow.com/questions/20975165/powerpoint-add-on-to-get-text-in-notes-in-slides-and-convert-it-to-audio-doesn/20981228
                        SlideRange notesPages = slide.NotesPage;
                        string slideName = "Slide" + slide.SlideIndex.ToString("D3") + ".jpg";
                        slide.Export(Path.Combine(destinationPathTmp,  slideName), "jpg");
                        if (slide.HasNotesPage == MsoTriState.msoTrue)
                        {
                            foreach (PowerPoint.Shape shape in notesPages.Shapes)
                            {
                                if (shape.Type == MsoShapeType.msoPlaceholder)
                                {
                                    if (shape.PlaceholderFormat.Type == PpPlaceholderType.ppPlaceholderBody)
                                    {
                                        string slideNote = ReplaceWordChars(shape.TextFrame.TextRange.Text);
                                        slideNote = slideNote.Replace("\r\n", "\n").Replace("\n", "<br />").Replace("\r", "<br />");
                                        Debug.WriteLine("Slide[" + slide.SlideIndex + "] Notes: [" + slideNote + "]");
                                        var newLine = string.Format("{0}|{1}", slideName, slideNote);
                                        csv.AppendLine(newLine);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var newLine = string.Format("{0},{1}", slideName, "");
                            csv.AppendLine(newLine);
                        }
                    }
                }
                string notesPath = Path.Combine(destinationPathTmp, @"notes.csv");
                File.WriteAllText(notesPath, csv.ToString(), Encoding.UTF8);
                string zipPath = destinationPath + ".zip";
                if (File.Exists(zipPath))
                {
                    FileInfo zipInfo = new FileInfo(zipPath);
                    zipInfo.Delete();
                }
                ZipFile.CreateFromDirectory(destinationPathTmp, zipPath);

                //using (FileStream zipFs = File.Open(zipPath, FileMode.Open))
                
                FileStream zipFs = File.Open(zipPath, FileMode.Open);
                string savedDeckDir = "";
                if (ReadDocumentProperty(pptPresentation, "PPT2Web dir") != null)
                {
                    savedDeckDir = ReadDocumentProperty(pptPresentation, "PPT2Web dir");
                    Debug.Print("xxx I already have a deckdir: " + savedDeckDir);
                }
                else
                    Debug.Print("xxx No savedDeckDir saved!!!");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var uploadStatus = UploadZipAsync(zipFs, zipPath, savedDeckDir, pptPresentation);


                ////////////////////////////// WAS HERE

                // Delete temporary folder:
                if (Directory.Exists(destinationPathTmp))
                {
                    DirectoryInfo dir = new DirectoryInfo(destinationPathTmp);
                    dir.Delete(true);
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Ooops! -> " + ex.ToString());
            }
        }

        private void saveDocumentProperty(Presentation pptPresentation, string prop, string value)
        {
            DocumentProperties properties;
            properties = pptPresentation.CustomDocumentProperties;
            if (ReadDocumentProperty(pptPresentation, prop) != null)
            {
                properties[prop].Delete();
            }
            properties.Add(prop, false, MsoDocProperties.msoPropertyTypeString, value);
        }

        private void clearDocumentProperty(Presentation pptPresentation, string prop)
        {
            DocumentProperties properties;
            properties = pptPresentation.CustomDocumentProperties;
            if (ReadDocumentProperty(pptPresentation, prop) != null)
            {
                properties[prop].Delete();
            }
        }

        private string ReadDocumentProperty(Presentation pptPresentation, string propertyName)
        {
            DocumentProperties properties;
            properties = pptPresentation.CustomDocumentProperties;
            foreach (DocumentProperty prop in properties)
            {
                if (prop.Name == propertyName)
                {
                    return prop.Value.ToString();
                }
            }
            return null;
        }

        private string ReplaceWordChars(string text)
        {
            var s = text;
            // smart single quotes and apostrophe
            s = Regex.Replace(s, "[\u2018\u2019\u201A]", "'");
            // smart double quotes
            s = Regex.Replace(s, "[\u201C\u201D\u201E]", "\"");
            // ellipsis
            s = Regex.Replace(s, "\u2026", "...");
            // dashes
            s = Regex.Replace(s, "[\u2013\u2014]", "-");
            // circumflex
            s = Regex.Replace(s, "\u02C6", "^");
            // open angle bracket
            s = Regex.Replace(s, "\u2039", "<");
            // close angle bracket
            s = Regex.Replace(s, "\u203A", ">");
            // spaces
            s = Regex.Replace(s, "[\u02DC\u00A0]", " ");

            return s;
        }


        private async Task UploadZipAsync(Stream zipFile, string fileName, string deckDir, Presentation pptPresentation)
        {
            HttpContent fileStreamContent = new StreamContent(zipFile);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.ExpectContinue = false;
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    if(deckDir == "" || deckDir == null)
                        formData.Add(new StringContent("none"), "deckDir");
                    else
                        formData.Add(new StringContent(deckDir), "deckDir");
                    try
                    {
                        HttpResponseMessage response = await client.PostAsync(url, formData);
                        string deckURL = await response.Content.ReadAsStringAsync();
                        URLbox.Text = deckURL;
                        URLbox.Enabled = true;
                        CopyToClipboard.Enabled = true;
                        deleteFromWeb.Enabled = true;
                        OpenInBrowser.Enabled = true;
                        PPT2Web.Enabled = true;
                        string[] tmp = deckURL.Split('/');
                        tmp = tmp[(tmp.Length - 1)].Split('=');
                        var webDeckDir = tmp[(tmp.Length - 1)];
                        saveDocumentProperty(pptPresentation, "PPT2Web URL", deckURL);
                        saveDocumentProperty(pptPresentation, "PPT2Web dir", webDeckDir);
                        Debug.Print("xxxx The deckDir: " + deckDir + " the URL: " + deckURL);
                    }
                    catch (Exception e)
                    {
                        Debug.Print("xxxx Houston!!!" );
                    }
                }
            }
        }

        public async Task<string> TestAPIGet()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    URLbox.Text = response.ToString();
                    return "YAY! Response: " + response.ToString();
                }
                catch (Exception ex)
                {
                    Debug.Print("Ooops! Exception when uploading (GET) -> " + ex.ToString());
                    URLbox.Text = ex.ToString();
                    return "XXX there was a problem again";
                }
            }
        }

        private void checkBox1_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void URLbox_TextChanged(object sender, RibbonControlEventArgs e)
        {

        }

        private void CopyToClipboard_Click(object sender, RibbonControlEventArgs e)
        {
            Clipboard.SetText(URLbox.Text);
        }

        private void OpenInBrowser_Click(object sender, RibbonControlEventArgs e)
        {
            if (Uri.TryCreate(URLbox.Text, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                try
                {
                    Process.Start(URLbox.Text);
                }
                catch (System.ComponentModel.Win32Exception noBrowser)
                {
                    if (noBrowser.ErrorCode == -2147467259)
                        MessageBox.Show(noBrowser.Message);
                }
                catch (Exception other)
                {
                    MessageBox.Show(other.Message);
                }
            }
        }

        private void deleteFromWeb_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void Settings_Click(object sender, RibbonControlEventArgs e)
        {
            settingsDialog.ShowDialog();
        }
    }
}
