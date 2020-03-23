using Epub2Kindle.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Epub2Kindle.Api.Controllers
{
    [ApiController]
    [Route("v1/[controller]")]
    public class ConversionController : ControllerBase
    {
        
        private readonly ILogger<ConversionController> _logger;

        public ConversionController(ILogger<ConversionController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("/upload")]
        public async Task<ActionResult<string>> Upload(IFormFile file)
        {
            if (file == null)
                return BadRequest();

            // Extract file name from whatever was posted by browser
            var fileName = Path.GetFileName(file.FileName);
            var tempPath = "./_tmp/";
            var requestId = Guid.NewGuid().ToString();

            System.IO.Directory.CreateDirectory(Path.Combine(tempPath, requestId));

            // Temp File Path
            var filePath = Path.Combine(tempPath, requestId, fileName);

            // If file with same name exists delete it
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }

            // Create new local file and copy contents of uploaded file
            using (var localFile = System.IO.File.OpenWrite(filePath))
            using (var uploadedFile = file.OpenReadStream())
            {
                await uploadedFile.CopyToAsync(localFile);
            }

            return Ok(filePath);
        }

        [HttpPost]
        [Route("/convert")]
        public async Task<ActionResult<string>> Convert(string fileName, string requestId)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var converter = new Converter();
            var tempPath = "./_tmp/";
            var outputFile = fileName.Replace(".epub", ".mobi");

            var result = converter.Convert(Path.Combine(tempPath, requestId), fileName, outputFile);

            if (result)
                return Ok(outputFile);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("/download")]
        public async Task<IActionResult> Download(string fileName, string requestId)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            var tempPath = "./_tmp/";

            // Temp File Path
            var filePath = Path.Combine(tempPath, requestId, fileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, GetContentType(filePath), Path.GetFileName(filePath));

        }

        [HttpPost]
        [Route("/mail")]
        public async Task<IActionResult> Mail(string fileName, string requestId, string mailTo, string mailFrom)
        {
            var tempPath = "./_tmp/";

            // Temp File Path
            var filePath = Path.Combine(tempPath, requestId, fileName);

            var mailer = new Mailer();
            var email = new Email();

            email.Host = "";            
            email.Port = 587;
            email.Password = "";

            email.To = mailTo;
            email.Address = mailFrom;
            email.Subject = fileName;
            email.AttachmentPaths = new List<string>();
            email.AttachmentPaths.Add(filePath);

            return Ok();
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".epub", "application/epub+zip"},
                {".mobi", "application/x-mobipocket-ebook"},
            };
        }
    }
}
