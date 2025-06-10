using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public interface ILocationRepo 
    {
        void Insert(Location location);
        IEnumerable<Location> GetAll();
        Location GetById(int id);
        void Update(Location location);
        void Delete(int id);
    }
}
