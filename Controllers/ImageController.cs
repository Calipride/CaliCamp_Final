using CaliCamp.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;
using CaliCamp.Models.Dtos;

namespace CaliCamp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepo _imageRepo;
        private readonly string _imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads");

        public ImageController(IImageRepo imageRepo)
        {
            _imageRepo = imageRepo;

            // Ensure the upload directory exists
            if (!Directory.Exists(_imageDirectory))
            {
                Directory.CreateDirectory(_imageDirectory);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var images = _imageRepo.GetAll();
            return Ok(images);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var image = _imageRepo.GetById(id);
            if (image == null)
            {
                return NotFound();
            }
            return Ok(image);
        }

        
        [HttpPost]
        [Produces("application/json")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Image))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] ImageUploadDto dto)
        {
            if (string.IsNullOrEmpty(dto.Base64Image))
            {
                return BadRequest("No image data provided.");
            }

            try
            {
                byte[] imageBytes = Convert.FromBase64String(dto.Base64Image);
                
                
                var fileName = $"{Guid.NewGuid()}_{dto.FileName}";
                var filePath = Path.Combine(_imageDirectory, fileName);

                
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

                var image = new Image
                {
                    CampingSpotId = dto.CampingSpotId,
                    UploadedBy = dto.UploadedBy,
                    UploadedAt = DateTime.UtcNow,
                    Url = $"/uploads/{fileName}",
                    FilePath = filePath
                };

                _imageRepo.Insert(image);
                return CreatedAtAction(nameof(Get), new { id = image.Id }, image);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid base64 string.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Image image)
        {
            if (id != image.Id)
            {
                return BadRequest();
            }

            _imageRepo.Update(image);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var existingImage = _imageRepo.GetById(id);
            if (existingImage == null)
            {
                return NotFound();
            }

            if (System.IO.File.Exists(existingImage.FilePath))
            {
                System.IO.File.Delete(existingImage.FilePath);
            }

            _imageRepo.Delete(id);
            return NoContent();
        }
    }
}
