
using AtbShop.Helpers;
using AtbShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;

namespace AtbShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadImage([FromBody] ImageViewModel dto)
        {
            try
            {
                var img = dto.Base64.FromBase64StringToImage();
                string randomFileName = Path.GetRandomFileName() + "{0}.jpg";
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFileName);

                img.Save(String.Format(dir, ""), ImageFormat.Jpeg);

                var img100 = img.Resize(100, 100);
                img100.Save(String.Format(dir, "_100"), ImageFormat.Jpeg);

                var img250 = img.Resize(250, 250);
                img250.Save(String.Format(dir, "_250"), ImageFormat.Jpeg);
                return Ok(new {imageUrl = String.Format(randomFileName, "") });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Title = ex.Message });
            }
        }
    }
}
