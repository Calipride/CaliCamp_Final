using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;


namespace CaliCamp.DataAccess
{
    public class LocationRepo : ILocationRepo
    {

        LiteDatabase db = new LiteDatabase(@"data.db");
        private const string _LOCATION = "locations";


        public void Insert(Location location)
        {
            db.GetCollection<Location>(_LOCATION).Insert(location);
        }

        public IEnumerable<Location> GetAll()
        {
            return db.GetCollection<Location>(_LOCATION).FindAll();
        }

        public Location GetById(int id)
        {
            return db.GetCollection<Location>(_LOCATION).FindById(id);
        }

        public void Update(Location location)
        {
            db.GetCollection<Location>(_LOCATION).Update(location);
        }

        public void Delete(int id)
        {
            db.GetCollection<Location>(_LOCATION).Delete(id);
        }

    }
}
