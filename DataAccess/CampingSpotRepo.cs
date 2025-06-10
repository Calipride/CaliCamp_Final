using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class CampingSpotRepo : ICampingSpotRepo
    {
        private readonly LiteDatabase _db;
        private const string _CAMPINGSPOT = "CampingSpots";

        public CampingSpotRepo(string connectionString = "Filename=MyData.db; Connection=shared")
        {
            try
            {
                _db = new LiteDatabase(connectionString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to database: {ex.Message}");
                throw;
            }
        }

        public void Insert(CampingSpot campingSpot)
        {
            try
            {
                _db.GetCollection<CampingSpot>(_CAMPINGSPOT).Insert(campingSpot);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting camping spot: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<CampingSpot> GetAll()
        {
            try
            {
                return _db.GetCollection<CampingSpot>(_CAMPINGSPOT).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving all camping spots: {ex.Message}");
                throw;
            }
        }

        public CampingSpot GetById(int id)
        {
            try
            {
                return _db.GetCollection<CampingSpot>(_CAMPINGSPOT).FindById(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving camping spot by ID: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<CampingSpot> GetByLocation(string location)
        {
            try
            {
                return _db.GetCollection<CampingSpot>(_CAMPINGSPOT)
                    .Find(x => x.Location == location)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving camping spots by location: {ex.Message}");
                throw;
            }
        }

        public void Update(CampingSpot campingSpot)
        {
            try
            {
                var exists = GetById(campingSpot.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Camping spot with ID {campingSpot.Id} not found");
                }
                _db.GetCollection<CampingSpot>(_CAMPINGSPOT).Update(campingSpot);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating camping spot: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var exists = GetById(id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Camping spot with ID {id} not found");
                }
                _db.GetCollection<CampingSpot>(_CAMPINGSPOT).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting camping spot: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<CampingSpot> GetByUserId(int userId)
        {
            try
            {
                return _db.GetCollection<CampingSpot>(_CAMPINGSPOT)
                    .Find(x => x.UserId == userId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving camping spots by user ID: {ex.Message}");
                throw;
            }
        }
    }
}
