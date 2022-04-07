
using AtbShop.Models;
using Microsoft.AspNetCore.Mvc;

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
                var bytes = Convert.FromBase64String(dto.Base64);
                string randomFilename = Path.GetRandomFileName() + ".jpg";
                string fileName = Path.Combine(Directory.GetCurrentDirectory(), "uploads", randomFilename);
                using (var imageFile = new FileStream(fileName, FileMode.Create))
                {
                    imageFile.Write(bytes, 0, bytes.Length);
                    imageFile.Flush();
                }
                return Ok(new {imageUrl= randomFilename });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { Title = ex.Message });
            }
        }
    }
}
