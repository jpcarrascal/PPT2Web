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
    public class UploadController : ControllerBase
    {
        private readonly IHostingEnvironment env;
        private readonly string uploadDirectory;
        public UploadController(IHostingEnvironment environment)
        {
            env = environment ?? throw new ArgumentNullException(nameof(environment));
            uploadDirectory = Path.Combine(env.ContentRootPath, "uploads");
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public ActionResult<string> Post([FromForm] FormDataModel formData)
        {
            ResponseModel response = new ResponseModel();
            response.status = "error";
            response.content = "Unknown error!";
            response = CreateOrUpdateSlideShow(formData, response, "create");
            var responseJson = JsonConvert.SerializeObject(response);
            return responseJson;
        }

        [HttpPut]
        [Consumes("multipart/form-data")]
        public ActionResult<string> Put([FromForm] FormDataModel formData)
        {
            ResponseModel response = new ResponseModel();
            response.status = "error";
            response.content = "Unknown error!";
            var slideShowPath = Path.Combine(uploadDirectory, formData.locator);
            if (formData.locator == "" || formData.locator == null)
            {
                response.status = "error";
                response.content = "No locator specified!";
            }
            else if (!Directory.Exists(slideShowPath))
            {
                response.status = "error";
                response.content = "'" + formData.locator + "' doesn't exist.";
            }
            else
            {
                response = CreateOrUpdateSlideShow(formData, response, "update");
            }
            var responseJson = JsonConvert.SerializeObject(response);
            return responseJson;
        }

        [HttpDelete]
        [Consumes("multipart/form-data")]
        public ActionResult<string> Delete([FromForm] FormDataModel formData)
        {
            ResponseModel response = new ResponseModel();
            response.status = "error";
            response.content = "Unknown error!";
            var slideShowPath = Path.Combine(uploadDirectory, formData.locator);
            if (formData.locator == "" || formData.locator == null)
            {
                response.status = "error";
                response.content = "No locator specified!";
            }
            else if (!Directory.Exists(slideShowPath))
            {
                response.status = "error";
                response.content = "'" + formData.locator + "' doesn't exist.";
            }
            else
            {
                try
                {
                    Directory.Delete(slideShowPath, true);
                    response.status = "success";
                    response.content = formData.locator + " removed successfully.";
                }
                catch (Exception e)
                {
                    response.status = "error";
                    response.content = "Can't delete '" + formData.locator + "': permission denied!.";
                }
            }
            var responseJson = JsonConvert.SerializeObject(response);
            return responseJson;
        }

        private ResponseModel CreateOrUpdateSlideShow(FormDataModel formData, ResponseModel response, string action)
        {
            if (formData.file != null && formData.file.Length > 0)
            {
                var locator = "";
                if (action == "create")
                    locator = GetUniqueID16();
                else
                    locator = formData.locator;
                var uniqueFileName = locator + Path.GetExtension(formData.file.FileName);
                var tempZipFilePath = Path.Combine(uploadDirectory, GetUniqueID16() + ".zip");
                var slideShowPath = Path.Combine(uploadDirectory, locator);
                try
                {
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
                catch (Exception e)
                {
                    response.status = "error";
                    response.content = "Error saving or updating! (Cannot find path)";
                }
            }
            else
            {
                response.status = "error";
                response.content = "No file provided in request!";
            }
            return response;
        }

        public class FormDataModel
        {
            public IFormFile file { get; set; }
            public string locator { get; set; }
            public string action { get; set; }
        }

        public class ResponseModel
        {
            public string status { get; set; }
            public string content { get; set; }
        }

        private string GetUniqueID16()
        {
            return Guid.NewGuid().ToString().Substring(0, 18);
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