using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public interface ICampingSpotRepo
    {
        void Insert(CampingSpot campingSpot);
        IEnumerable<CampingSpot> GetAll();
        CampingSpot GetById(int id);
        void Update(CampingSpot campingSpot);
        void Delete(int id);


        IEnumerable<CampingSpot> GetByUserId(int userId);

    }
}
