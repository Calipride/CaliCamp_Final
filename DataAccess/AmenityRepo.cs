using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class AmenityRepo : IAmenityRepo
    {
        private readonly LiteDatabase _db;
        private const string _AMENITIES = "Amenities";

        public AmenityRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Amenity amenity)
        {
            try
            {
                if (amenity == null) throw new ArgumentNullException(nameof(amenity));
                _db.GetCollection<Amenity>(_AMENITIES).Insert(amenity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting amenity: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Amenity> GetAll()
        {
            try
            {
                return _db.GetCollection<Amenity>(_AMENITIES).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving amenities: {ex.Message}");
                throw;
            }
        }

        public Amenity GetById(int id)
        {
            try
            {
                var amenity = _db.GetCollection<Amenity>(_AMENITIES).FindById(id);
                if (amenity == null)
                {
                    throw new KeyNotFoundException($"Amenity with ID {id} not found");
                }
                return amenity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving amenity: {ex.Message}");
                throw;
            }
        }

        public void Update(Amenity amenity)
        {
            try
            {
                if (amenity == null) throw new ArgumentNullException(nameof(amenity));
                var exists = GetById(amenity.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Amenity with ID {amenity.Id} not found");
                }
                _db.GetCollection<Amenity>(_AMENITIES).Update(amenity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating amenity: {ex.Message}");
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
                    throw new KeyNotFoundException($"Amenity with ID {id} not found");
                }
                _db.GetCollection<Amenity>(_AMENITIES).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting amenity: {ex.Message}");
                throw;
            }
        }
    }
}
