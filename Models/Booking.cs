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

        // Relationships - made nullable since they're navigation properties
        public CampingSpot? CampingSpot { get; set; } // Many-to-one relationship with CampingSpot
        public User? User { get; set; } // Many-to-one relationship with User
    }
}
