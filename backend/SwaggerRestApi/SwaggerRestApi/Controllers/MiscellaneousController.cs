using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SwaggerRestApi.BusineesLogic;
using SwaggerRestApi.Models.DTO;
using SwaggerRestApi.Models.DTO.Barcode;

namespace SwaggerRestApi.Controllers
{
    [ApiController]
    public class MiscellaneousController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly SharedLogic _sharedlogic;

        public MiscellaneousController(IConfiguration configuration, SharedLogic sharedLogic)
        {
            _configuration = configuration;
            _sharedlogic = sharedLogic;
        }


        [HttpPost("/images")]
        [Authorize(Roles = "Admin, Operator")]
        public async Task<ActionResult<ImagePost>> SaveImage(IFormFile image)
        {
            string imageBasePath = _configuration["ImageSavePath"];

            if (image == null || image.Length == 0) { return BadRequest(new { message = "Could not find image" }); }
            if (Path.GetExtension(image.FileName).ToLower() != ".png" && Path.GetExtension(image.FileName).ToLower() != ".jpeg"
                && Path.GetExtension(image.FileName).ToLower() != ".jpg") { return BadRequest(new { message = "Invalid file format" }); }

            if (!System.IO.File.Exists(imageBasePath))
            {
                Directory.CreateDirectory(imageBasePath);
            }

            var fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, Path.GetExtension(image.FileName));

            var filePath = Path.Combine(imageBasePath, fileName);

            using (var stream = System.IO.File.Create(filePath)) { await image.CopyToAsync(stream); }

            return Ok(fileName);
        }

        [HttpGet("/images/{filename}")]
        public async Task<ActionResult> GetImage(string filename)
        {
            string imageBasePath = _configuration["ImageSavePath"];
            string filePath = Path.Combine(imageBasePath, filename);

            if (!System.IO.File.Exists(filePath)) { return BadRequest(new { message = "Could not find image" }); }

            var extension = Path.GetExtension(filePath);

            if (extension.ToLower() == ".jpg") { extension = ".jpeg"; }
            var mimeType = $"image/{extension.Substring(1)}";

            return PhysicalFile(filePath, mimeType, filename);
        }


        [HttpPost("/barcodes/scan")]
        [Authorize(Roles = "Admin, Operator, User")]
        public async Task<ActionResult<ScannedBarcode>> GetBarcodeItem(string barcode)
        {
            return await _sharedlogic.GetScannedItem(barcode);
        }
    }
}
