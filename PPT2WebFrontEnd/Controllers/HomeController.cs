using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPT2WebFrontEnd.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.RegularExpressions;

namespace PPT2WebFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private string url;
        private readonly IHostingEnvironment env;

        public HomeController(IHostingEnvironment environment)
        {
            env = environment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (env.IsDevelopment())
                url = "https://localhost:44354/images/";
            else
                url = "https://ppt2webuploadservice.azurewebsites.net/uploads/";
            List<string> images = new List<string>();
            List<string> notes = new List<string>();
            if (!String.IsNullOrEmpty(Request.Query["deck"]))
            {
                string deckLoc = Request.Query["deck"];
                var deckPath = url + deckLoc;
                string slideNoteFile;
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        slideNoteFile = client.DownloadString(deckPath + "/notes.csv");
                        String[] slideNoteList = slideNoteFile.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                        int slideNoteCount = slideNoteList.Length;
                        for (int i = 0; i < slideNoteCount; i++)
                        {
                            var line = slideNoteList[i];
                            var values = line.Split('|');
                            if (values.Length > 1)
                            {
                                images.Add(deckPath + "/" + values[0]);
                                notes.Add(ReplaceWordChars(values[1]));
                            }
                        }
                        ViewData["errorClass"] = "hideError";
                    }
                    catch (Exception e)
                    {
                        Debug.Print("CSV not found!");
                    }
                }
            }
            else
            {
                ViewData["errorClass"] = "showError";
            }

            ViewData["images"] = images;
            ViewData["notes"] = notes;
            ViewData["slideCount"] = notes.Count;
            return View();
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

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
