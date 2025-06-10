using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models
{
    public class Amenity 
    {
        public int Id { get; set; } // Unique identifier for the amenity
        public string Name { get; set; } = ""; // Name of the amenity
        public string Description { get; set; } = "";  // Description of the amenity


    }
}
