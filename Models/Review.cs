using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    public class Review
    {
        public int Id { get; set; } // Unique identifier for the review
        public int CampingSpotId { get; set; } // Foreign key 
        public int UserId { get; set; } // Foreign key
        public int Rating { get; set; } // Numeric rating given by the user
        public string Comment { get; set; } = "";
        public DateTime CreatedAt { get; set; }

        // Relationships
        public CampingSpot CampingSpot { get; set; } = new CampingSpot();  // Many-to-one relationship with CampingSpot
        public User User { get; set; } = new User(); // Many-to-one relationship with User
    }

}
