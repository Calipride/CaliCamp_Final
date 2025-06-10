using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    /// <summary>
    /// Represents an image associated with a camping spot
    /// </summary>
    public class Image 
    {
        /// <summary>
        /// The unique identifier for the image
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The ID of the camping spot this image belongs to
        /// </summary>
        public int CampingSpotId { get; set; }

        /// <summary>
        /// The URL where the image can be accessed
        /// </summary>
        public string Url { get; set; } = "";

        /// <summary>
        /// The ID of the user who uploaded the image
        /// </summary>
        public int UploadedBy { get; set; }

        /// <summary>
        /// The date and time when the image was uploaded
        /// </summary>
        public DateTime UploadedAt { get; set; }

        /// <summary>
        /// The camping spot this image belongs to
        /// </summary>
        public CampingSpot CampingSpot { get; set; } = new CampingSpot();

        /// <summary>
        /// The user who uploaded the image
        /// </summary>
        public User? UploadedByUser { get; set; } = new User();

        /// <summary>
        /// The physical file path of the image on the server
        /// </summary>
        public string FilePath { get; set; } = "";
    }
}
