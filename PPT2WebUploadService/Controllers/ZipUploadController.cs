using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
//using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PPT2WebUploadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipUploadController : ControllerBase
    {
        private readonly IHostingEnvironment env;
        private readonly string uploadDirectory;
        public ZipUploadController(IHostingEnvironment environment)
        {
            env = environment ?? throw new ArgumentNullException(nameof(environment));
            uploadDirectory = Path.Combine(env.ContentRootPath, "uploads");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public /*async Task*/ ActionResult<string> Post([FromForm] FormDataModel formData)
        {
            ResponseModel response = new ResponseModel();
            response.status = "error";
            response.content = "Unknown error!";
            if(formData.command != "" && formData.command != null)
            {
                switch (formData.command)
                {
                    case "create":
                        response = CreateOrUpdateSlideShow(formData, response);
                        break;
                    case "update":
                        response = CreateOrUpdateSlideShow(formData, response);
                        break;
                    case "delete":
                        response = DeleteSlideShow(formData, response);
                        break;
                    default:
                        response.status = "error";
                        response.content = "Wrong command specified!";
                        break;
                }
            }
            else
            {
                response.status = "error";
                response.content = "No command specified!";
            }
            var responseJson = JsonConvert.SerializeObject(response);
            return responseJson;
        }

        private ResponseModel CreateOrUpdateSlideShow(FormDataModel formData, ResponseModel response)
        {
            try
            {
                if (formData.file != null && formData.file.Length > 0)
                {
                    var uniqueFileName = "";
                    if (formData.locator == "none" || formData.locator == null)
                        uniqueFileName = GetUniqueFileName(formData.file.FileName);
                    else
                        uniqueFileName = formData.locator + Path.GetExtension(formData.file.FileName);
                    var tempZipFilePath = Path.Combine(uploadDirectory, GetUniqueFileName(uniqueFileName));
                    var locator = Path.GetFileNameWithoutExtension(uniqueFileName);
                    var slideShowPath = Path.Combine(uploadDirectory, locator);
                    if (Directory.Exists(slideShowPath))
                        Directory.Delete(slideShowPath, true);
                    using (var fileStream = new FileStream(tempZipFilePath, FileMode.Create))
                    {
                        /*await file.CopyToAsync(fileStream);*/
                        formData.file.CopyTo(fileStream);
                    }
                    try
                    {
                        ZipFile.ExtractToDirectory(tempZipFilePath, slideShowPath);
                    }
                    catch (Exception e)
                    {
                        response.status = "error";
                        response.content = "Error extracting zip file in server!";
                    }
                    if (System.IO.File.Exists(tempZipFilePath))
                        System.IO.File.Delete(tempZipFilePath);
                    response.status = "success";
                    response.content = locator;
                }
                else
                {
                    response.status = "error";
                    response.content = "No file specified!";
                }
            }
            catch (Exception e)
            {
                response.status = "error";
                response.content = "Error saving or updating! (Cannot find path)";
            }
            return response;
        }

        private ResponseModel DeleteSlideShow(FormDataModel formData, ResponseModel response)
        {
            if (formData.locator != null && formData.locator != "")
            {
                var slideShowPath = Path.Combine(uploadDirectory, formData.locator);
                if (Directory.Exists(slideShowPath))
                {
                    Directory.Delete(slideShowPath, true);
                    response.status = "success";
                    response.content = formData.locator + " removed successfully.";
                }
                else
                {
                    response.status = "success";
                    response.content = formData.locator + " does not exist.";
                }
            }
            else
            {
                response.status = "error";
                response.content = "No locator specified!";
            }
            return response;
        }

        public class FormDataModel
        {
            public IFormFile file { get; set; }
            public string locator { get; set; }
            public string command { get; set; }
        }

        public class ResponseModel
        {
            public string status { get; set; }
            public string content { get; set; }
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
            ResponseModel response = new ResponseModel();
            response.status = "success";
            response.content = "test";
            var responseJson = JsonConvert.SerializeObject(response);
            return responseJson;
        }
    }
}