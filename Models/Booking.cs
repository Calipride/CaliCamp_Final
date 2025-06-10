using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    public class Booking 
    {
        public int Id { get; set; }
        public int CampingSpotId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumGuests { get; set; }
        public decimal TotalPrice { get; set; }

        // Relationships
        public CampingSpot CampingSpot { get; set; } = new CampingSpot(); // Many-to-one relationship with CampingSpot
        public User User { get; set; } = new User(); // Many-to-one relationship with User


    }
}
