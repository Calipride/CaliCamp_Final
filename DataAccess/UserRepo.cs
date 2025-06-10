using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class UserRepo : IUserRepo
    {
        private readonly LiteDatabase _db;
        private const string _USERS = "Users";

        public UserRepo(string connectionString)
        {
            _db = new LiteDatabase(connectionString);
        }

        public void Insert(User user)
        {
            try
            {
                var existingUser = GetByEmail(user.Email);
                if (existingUser != null)
                {
                    throw new Exception("User with the same email address already exists");
                }

                _db.GetCollection<User>(_USERS).Insert(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting user: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                return _db.GetCollection<User>(_USERS).FindAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving users: {ex.Message}");
                throw;
            }
        }

        public User GetById(int id)
        {
            try
            {
                return _db.GetCollection<User>(_USERS).FindById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by ID: {ex.Message}");
                throw;
            }
        }

        public void Update(User user)
        {
            try
            {
                _db.GetCollection<User>(_USERS).Update(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                _db.GetCollection<User>(_USERS).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                throw;
            }
        }

        public User? GetByEmail(string email)
        {
            try
            {
                return _db.GetCollection<User>(_USERS).Find(u => u.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving user by email: {ex.Message}");
                throw;
            }
        }

        // Additional methods as needed for admin features
        public IEnumerable<User> GetAdminUsers()
        {
            try
            {
                return _db.GetCollection<User>(_USERS).Find(u => u.IsAdmin).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving admin users: {ex.Message}");
                throw;
            }
        }
    }
}

