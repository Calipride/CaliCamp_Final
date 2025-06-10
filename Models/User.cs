using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace CaliCamp.Models
{
    public class User 
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string ProfilePicturePath { get; set; } = "";

        public bool IsAdmin { get; set; }


        public List<Booking> Bookings { get; set; }
        public List<Review> Reviews { get; set; }

        public User()
        {
            Bookings = new List<Booking>();
            Reviews = new List<Review>();
        }
    }
}

