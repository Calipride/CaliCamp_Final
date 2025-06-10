using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public interface IUserRepo
    {
        User? GetByEmail(string email);
        User GetById(int id);
        IEnumerable<User> GetAll();
        void Insert(User user);
        void Update(User user);
        void Delete(int id);
    }
}
