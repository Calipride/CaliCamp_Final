using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public interface IAmenityRepo 
    {
        void Insert(Amenity amenity);
        IEnumerable<Amenity> GetAll();
        Amenity GetById(int id);
        void Update(Amenity amenity);
        void Delete(int id);
    }
}
