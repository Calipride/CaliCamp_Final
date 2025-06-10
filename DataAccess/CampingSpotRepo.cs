using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class CampingSpotRepo : ICampingSpotRepo
    {
        LiteDatabase db = new LiteDatabase(@"data.db");
        private const string _CAMPINGSPOT = "CampingSpots";


        public void Insert(CampingSpot campingSpot)
        {
            db.GetCollection<CampingSpot>(_CAMPINGSPOT).Insert(campingSpot);
        }

        public IEnumerable<CampingSpot> GetAll()
        {
            return db.GetCollection<CampingSpot>(_CAMPINGSPOT).FindAll();
        }

        public CampingSpot GetById(int id)
        {
            return db.GetCollection<CampingSpot>(_CAMPINGSPOT).FindById(id);
        }

        public IEnumerable<CampingSpot> GetByLocation(string location)
        {
            return db.GetCollection<CampingSpot>(_CAMPINGSPOT).Find(x => x.Location == location);
        }

        public void Update(CampingSpot campingSpot)
        {
            db.GetCollection<CampingSpot>(_CAMPINGSPOT).Update(campingSpot);
        }

        public void Delete(int id)
        {
            db.GetCollection<CampingSpot>(_CAMPINGSPOT).Delete(id);
        }

        public IEnumerable<CampingSpot> GetByUserId(int userId)
        {
            return db.GetCollection<CampingSpot>(_CAMPINGSPOT).Find(x => x.UserId == userId);
        }

    }
}
