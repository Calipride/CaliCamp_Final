using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CaliCamp.Models.Dtos
{
    /// <summary>
    /// Data transfer object for uploading images
    /// </summary>
    public class ImageUploadDto
    {
        /// <summary>
        /// The base64 encoded image data. Should include the data URI scheme (e.g., data:image/jpeg;base64,)
        /// </summary>
        /// <example>data:image/jpeg;base64,/9j/4AAQSkZJRg...</example>
        [Required]
        public string Base64Image { get; set; } = string.Empty;
        
        /// <summary>
        /// The name of the image file, including extension
        /// </summary>
        /// <example>campsite.jpg</example>
        [Required]
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// The ID of the camping spot this image belongs to
        /// </summary>
        /// <example>1</example>
        [Required]
        public int CampingSpotId { get; set; }
        
        /// <summary>
        /// The ID of the user who is uploading the image
        /// </summary>
        /// <example>1</example>
        [Required]
        public int UploadedBy { get; set; }
        
        /// <summary>
        /// Optional description of the image
        /// </summary>
        /// <example>Beautiful view of the campsite</example>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The uploaded file (for multipart/form-data uploads)
        /// </summary>
        public IFormFile? File { get; set; }
    }
}
