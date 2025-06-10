using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    public class Image 
    {
        
        public int Id { get; set; }

        
        public int CampingSpotId { get; set; }

        public string Url { get; set; } = "";
        public int UploadedBy { get; set; }
        public DateTime UploadedAt { get; set; }
        public CampingSpot CampingSpot { get; set; } = new CampingSpot();
        public User? UploadedByUser { get; set; } = new User();
        public string FilePath { get; set; } = "";
    }
}
