using CaliCamp.DataAccess;
using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace CaliCamp.Services
{

    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepo _userRepo; // Change object to IUserRepository

        public UserService(IConfiguration config, IUserRepo userRepo)
        {
            _config = config;
            _userRepo = userRepo; // Assign the injected userRepository
        }

        public User Authenticate(string email, string password)
        {
            var user = _userRepo.GetByEmail(email); // Use the IUserRepository method

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new AuthenticationException("Invalid email or password"); // Throw an exception if authentication fails
            }

            return user;
        }


        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("sample_secret_key_123456");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

