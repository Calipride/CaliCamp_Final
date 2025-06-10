using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using static System.Net.Mime.MediaTypeNames;

namespace CaliCamp.Models
{
    public class CampingSpot
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Location { get; set; } = "";
        public decimal PricePerNight { get; set; }
        public string Description { get; set; } = "";
        public List<string> Amenities { get; set; }
        public int MaxGuests { get; set; }
        public List<DateTime> AvailableDates { get; set; }
        public List<Booking> Bookings { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Image> Images { get; set; }

        // New property to store the ID of the user who created this camping spot
        public int UserId { get; set; }

        public CampingSpot()
        {
            Amenities = new List<string>();
            AvailableDates = new List<DateTime>();
            Bookings = new List<Booking>();
            Reviews = new List<Review>();
            Images = new List<Image>();
        }
    }


}
