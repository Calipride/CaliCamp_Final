using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    public class Location 
    {
        public int Id { get; set; } // Unique identifier for the location
        public string Name { get; set; } = ""; // Name of the location
        public double Latitude { get; set; } // Latitude coordinate of the location
        public double Longitude { get; set; } // Longitude coordinate of the location

        public Location()
        {
        }
    }
}
