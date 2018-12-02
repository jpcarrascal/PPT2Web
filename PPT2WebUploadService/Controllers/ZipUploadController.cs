using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
//using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;

namespace PPT2WebUploadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipUploadController : ControllerBase
    {
        private readonly IHostingEnvironment env;
        public ZipUploadController(IHostingEnvironment environment)
        {
            env = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public /*async Task*/ ActionResult<string> Post([FromForm] FileInputModel formData)
        {
            var uploads = Path.Combine(env.ContentRootPath, "uploads");
            try
            {
                if (formData.file != null && formData.file.Length > 0)
                {
                    var uniqueFileName = "";
                    if (formData.deckDir == "none" || formData.deckDir == null)
                        uniqueFileName = GetUniqueFileName(formData.file.FileName);
                    else
                        uniqueFileName = formData.deckDir + Path.GetExtension(formData.file.FileName);
                    var tempZipFilePath = Path.Combine(uploads, GetUniqueFileName(uniqueFileName));
                    var extractPath = Path.Combine(uploads, Path.GetFileNameWithoutExtension(uniqueFileName));
                    if (Directory.Exists(extractPath))
                        Directory.Delete(extractPath, true);
                    using (var fileStream = new FileStream(tempZipFilePath, FileMode.Create))
                    {
                        /*await file.CopyToAsync(fileStream);*/
                        formData.file.CopyTo(fileStream);
                    }
                    try
                    {
                        ZipFile.ExtractToDirectory(tempZipFilePath, extractPath);
                    }
                    catch (Exception e) {
                        Debug.Print("xxxx TempZIP: " + tempZipFilePath + " Extract to" + extractPath);
                    }
                    if (System.IO.File.Exists(tempZipFilePath))
                        System.IO.File.Delete(tempZipFilePath);
                    var deckUrl = "";
                    if (env.IsDevelopment())
                        deckUrl = HttpContext.Request.Host.ToString() + "/uploads/" + Path.GetFileNameWithoutExtension(uniqueFileName);
                    else
                        deckUrl = "https://ppt2webfrontend.azurewebsites.net/?deck=" + Path.GetFileNameWithoutExtension(uniqueFileName);
                    return deckUrl;
                }
                else
                    return "ERROR1: no file especified!";
            }
            catch (Exception e)
            {
                return "ERROR0: "+e.ToString();
            }
        }

        public class FileInputModel
        {
            public IFormFile file { get; set; }
            public string deckDir { get; set; }
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 8)
                      + Path.GetExtension(fileName);
        }

        [HttpGet]
        public ActionResult<string> Get(int id)
        {
            //return "Path: " + Path.Combine(env.WebRootPath, "uploads");
            return "Test..." + id;
        }
    }
}