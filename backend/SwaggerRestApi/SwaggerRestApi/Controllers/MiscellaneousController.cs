using BarcodeStandard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
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
        public async Task<ActionResult<ImagePost>> SaveImage()
        {
            string imageBasePath = _configuration["ImageSavePath"];

            if (HttpContext.Request.ContentType != "image/png" && HttpContext.Request.ContentType != "image/jpeg") { return BadRequest(new { message = "Invalid file format" }); }

            if (!System.IO.File.Exists(imageBasePath))
            {
                Directory.CreateDirectory(imageBasePath);
            }

            string extension = HttpContext.Request.ContentType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
            };

            var fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, extension);

            var filePath = Path.Combine(imageBasePath, fileName);

            using (var stream = System.IO.File.Create(filePath)) { await Request.Body.CopyToAsync(stream); }

            return Ok(new ImagePost { path = fileName });
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

        [HttpGet("/barcodes/generate")]
        public async Task<ActionResult> GenerateBarcode(string barcode)
        {
            if (!barcode.All(char.IsDigit) || barcode.Length != 13) { return BadRequest("EAN-13 requires 13 numeric digits."); }

            var b = new Barcode();
            b.IncludeLabel = true;
            b.LabelFont = new SKFont{
                Typeface = SKTypeface.FromFile("./Fonts/Roboto.ttf"),
                Size = 28,
            };
            SKImage img = b.Encode(BarcodeStandard.Type.Ean13, barcode, 300, 150);

            var encoded = img.Encode();

            Stream stream = encoded.AsStream();

            return File(stream, "image/png");
        }
    }
}
