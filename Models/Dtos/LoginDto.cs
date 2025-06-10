using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.Models.Dtos
{
    public class LoginDto
    {
        public string? Email { get; set; } // Declare as nullable
        public string? Password { get; set; } // Declare as nullable
    }
}
