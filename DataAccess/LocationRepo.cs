using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class LocationRepo : ILocationRepo
    {
        private readonly LiteDatabase _db;
        private const string _LOCATIONS = "Locations";

        public LocationRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Location location)
        {
            try
            {
                if (location == null) throw new ArgumentNullException(nameof(location));
                _db.GetCollection<Location>(_LOCATIONS).Insert(location);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting location: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Location> GetAll()
        {
            try
            {
                return _db.GetCollection<Location>(_LOCATIONS).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving locations: {ex.Message}");
                throw;
            }
        }

        public Location GetById(int id)
        {
            try
            {
                var location = _db.GetCollection<Location>(_LOCATIONS).FindById(id);
                if (location == null)
                {
                    throw new KeyNotFoundException($"Location with ID {id} not found");
                }
                return location;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving location: {ex.Message}");
                throw;
            }
        }

        public void Update(Location location)
        {
            try
            {
                if (location == null) throw new ArgumentNullException(nameof(location));
                var exists = GetById(location.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Location with ID {location.Id} not found");
                }
                _db.GetCollection<Location>(_LOCATIONS).Update(location);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating location: {ex.Message}");
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
                    throw new KeyNotFoundException($"Location with ID {id} not found");
                }
                _db.GetCollection<Location>(_LOCATIONS).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting location: {ex.Message}");
                throw;
            }
        }
    }
}
