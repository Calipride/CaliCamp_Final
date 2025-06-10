using CaliCamp.Models;

namespace CaliCamp.Services;

public interface IUserService
{
User Authenticate(string email, string password);
string GenerateJwtToken(User user);
}