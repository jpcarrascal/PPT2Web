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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PPT2WebVSTO
{
    /*
     HKEY_CURRENT_USER\Software\Microsoft\VSTA\Solutions
    */
    public partial class Ribbon1
    {
        private string apiURL;
        private string uploadURL;
        public SettingsDialog settingsDialog;
        private void Ribbon1_Load(object sender, RibbonUIEventArgs e)
        {
            settingsDialog = new SettingsDialog(Properties.Settings.Default.uploadURL, Properties.Settings.Default.showURL);
            PowerPoint.Application app = Globals.ThisAddIn.Application;
            apiURL = Properties.Settings.Default.uploadURL;
            if (!apiURL.EndsWith("/"))
                apiURL = apiURL + "/";
            uploadURL = apiURL + "upload/";
            app.AfterPresentationOpen += AfterPresentationOpenHandle;
            app.AfterNewPresentation += AfterPresentationOpenHandle;
            app.PresentationBeforeClose += ResetControls;
            app.WindowActivate += SwitchWindowsHandle;
            // Need to detect "save as..."
            //app.PresentationBeforeSave += clearSavedProperties;
        }

        private void SwitchWindowsHandle(Presentation pres, DocumentWindow window)
        {
            PPT2Web.Enabled = true;
            Presentation pptPresentation = window.Presentation;
            if (ReadDocumentProperty(pptPresentation, "PPT2Web locator") != null)
            {
                Locator.Text = ReadDocumentProperty(pptPresentation, "PPT2Web locator");
                //Locator.Enabled = true;
                CopyToClipboard.Enabled = true;
                deleteFromWeb.Enabled = true;
                OpenInBrowser.Enabled = true;
            }
            else
            {
                Locator.Text = "";
                Locator.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
            }

        }

        private void AfterPresentationOpenHandle(Presentation pptPresentation)
        {
            PPT2Web.Enabled = true;
            if (ReadDocumentProperty(pptPresentation, "PPT2Web locator") != null)
            {
                Locator.Text = ReadDocumentProperty(pptPresentation, "PPT2Web locator");
                //Locator.Enabled = true;
                CopyToClipboard.Enabled = true;
                deleteFromWeb.Enabled = true;
                OpenInBrowser.Enabled = true;
            }
            else
            {
                Locator.Text = "";
                Locator.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
            }
        }

        private void ResetControls(Presentation pres, ref bool cancel)
        {
            Locator.Text = "";
            Locator.Enabled = false;
            CopyToClipboard.Enabled = false;
            deleteFromWeb.Enabled = false;
            OpenInBrowser.Enabled = false;
            PPT2Web.Enabled = false;
        }

        private void ClearSavedProperties(Presentation pres,  ref bool cancel)
        {
            Locator.Text = "";
            Locator.Enabled = false;
            CopyToClipboard.Enabled = false;
            deleteFromWeb.Enabled = false;
            OpenInBrowser.Enabled = false;
            PPT2Web.Enabled = true;
            ClearDocumentProperty(pres, "PPT2Web locator");
        }

        private void Publish2Web_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Presentation pptPresentation = Globals.ThisAddIn.GetActiveDeck();
                Locator.Text = "Publishing...";
                Locator.Enabled = false;
                CopyToClipboard.Enabled = false;
                deleteFromWeb.Enabled = false;
                OpenInBrowser.Enabled = false;
                PPT2Web.Enabled = false;
                Settings.Enabled = false;
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
                string savedLocator = "";
                if (ReadDocumentProperty(pptPresentation, "PPT2Web locator") != null)
                {
                    savedLocator = ReadDocumentProperty(pptPresentation, "PPT2Web locator");
                    Debug.Print("xxx I already have a locator: " + savedLocator);
                }
                else
                    Debug.Print("xxx No savedLocator saved!!!");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var uploadStatus = UploadZipAsync(zipFs, zipPath, savedLocator, pptPresentation);

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

        private void RemoveFromWeb_Click(object sender, RibbonControlEventArgs e)
        {
            Presentation pptPresentation = Globals.ThisAddIn.GetActiveDeck();
            if (ReadDocumentProperty(pptPresentation, "PPT2Web locator") != null)
            {
                string savedLocator = ReadDocumentProperty(pptPresentation, "PPT2Web locator");
                Debug.Print("xxx I do have a locator: " + savedLocator);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var uploadStatus = RemoveDeckAsync(savedLocator, pptPresentation);
            }
            else
            {
                Debug.Print("No locator saved with the Powerpoint document!!!");
            }
        }


        private async Task UploadZipAsync(Stream zipFile, string fileName, string locator, Presentation pptPresentation)
        {
            HttpContent fileStreamContent = new StreamContent(zipFile);
            fileStreamContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data") { Name = "file", FileName = fileName };
            fileStreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            using (var client = new HttpClient())
            {
                //client.DefaultRequestHeaders.ExpectContinue = false;
                var method = HttpMethod.Post;
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(fileStreamContent);
                    if (locator == "" || locator == null)
                    {
                        formData.Add(new StringContent("create"), "action");
                        formData.Add(new StringContent(""), "locator");
                        method = HttpMethod.Post;
                    }
                    else
                    {
                        formData.Add(new StringContent("update"), "action");
                        formData.Add(new StringContent(locator), "locator");
                        method = HttpMethod.Put;
                    }
                    try
                    {
                        var request = new HttpRequestMessage(method, uploadURL);
                        request.Content = formData;
                        HttpResponseMessage response = await client.SendAsync(request);
                        //HttpResponseMessage response = await client.PostAsync(uploadURL, formData);
                        string responseJson = await response.Content.ReadAsStringAsync();
                        ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseJson);
                        if (responseModel.status == "success")
                        {
                            string webLocator = responseModel.content;
                            Locator.Text = webLocator;
                            //Locator.Enabled = true;
                            CopyToClipboard.Enabled = true;
                            deleteFromWeb.Enabled = true;
                            OpenInBrowser.Enabled = true;
                            PPT2Web.Enabled = true;
                            Settings.Enabled = true;
                            try
                            {
                                SaveDocumentProperty(pptPresentation, "PPT2Web locator", webLocator);
                            }
                            catch (Exception e)
                            {
                                Debug.Print("WARNING: CANNOT SAVE PROPERTIES!!!" + e.ToString());
                            }
                        }
                        else
                        {
                            if (responseModel.status != null && responseModel.status != "")
                                Locator.Text = "ERROR: " + responseModel.status;
                            else
                                Locator.Text = "Unknown error!";
                        }
                        if (File.Exists(fileName))
                        {
                            Debug.Print("Deleting temporary file...");
                            FileInfo zipInfo = new FileInfo(fileName);
                            zipInfo.Delete();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Print("xxxx Houston!!!" + e.ToString() );
                    }
                }
            }
        }

        private async Task RemoveDeckAsync(string locator, Presentation pptPresentation)
        {
            using (var client = new HttpClient())
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent("delete"), "action");
                formData.Add(new StringContent(locator), "locator");
                try
                {
                    var method = HttpMethod.Delete;
                    var request = new HttpRequestMessage(method, uploadURL);
                    request.Content = formData;
                    HttpResponseMessage response = await client.SendAsync(request);
                    //HttpResponseMessage response = await client.PostAsync(uploadURL, formData);
                    string responseJson = await response.Content.ReadAsStringAsync();
                    ResponseModel responseModel = JsonConvert.DeserializeObject<ResponseModel>(responseJson);
                    if(responseModel.status == "success") //success
                    {
                        Locator.Text = "";
                    }
                    else
                    {
                        Locator.Text = responseModel.content;
                    }
                    Locator.Enabled = false;
                    CopyToClipboard.Enabled = false;
                    deleteFromWeb.Enabled = false;
                    OpenInBrowser.Enabled = false;
                    PPT2Web.Enabled = true;
                    ClearDocumentProperty(pptPresentation, "PPT2Web locator");
                }
                catch (Exception e)
                {
                    Debug.Print("xxxx Houston!!!" + e.ToString());
                }

            }
        }

        private void CheckBox1_Click(object sender, RibbonControlEventArgs e)
        {

        }

        private void CopyToClipboard_Click(object sender, RibbonControlEventArgs e)
        {
            string url = Properties.Settings.Default.showURL + Locator.Text;
            Clipboard.SetText(url);
        }

        private void OpenInBrowser_Click(object sender, RibbonControlEventArgs e)
        {
            string url = Properties.Settings.Default.showURL + Locator.Text;
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                try
                {
                    Process.Start(url);
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

        private void Settings_Click(object sender, RibbonControlEventArgs e)
        {
            settingsDialog.ShowDialog();
        }

        private void SaveDocumentProperty(Presentation pptPresentation, string prop, string value)
        {
            if (ReadDocumentProperty(pptPresentation, prop) != null)
            {
                pptPresentation.CustomDocumentProperties[prop].Delete();
            }
            pptPresentation.CustomDocumentProperties.Add(prop, false, MsoDocProperties.msoPropertyTypeString, value);
        }

        private void ClearDocumentProperty(Presentation pptPresentation, string prop)
        {
            if (ReadDocumentProperty(pptPresentation, prop) != null)
            {
                pptPresentation.CustomDocumentProperties[prop].Delete();
            }
        }

        private string ReadDocumentProperty(Presentation pptPresentation, string propertyName)
        {
            try
            {
                return pptPresentation.CustomDocumentProperties[propertyName].Value.ToString();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public class ResponseModel
        {
            public string status { get; set; }
            public string content { get; set; }
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

    }
}
