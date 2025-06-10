using CaliCamp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public interface IBookingRepo 
    {
        void Insert(Booking booking);
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
        void Update(Booking booking);
        void Delete(int id);

    }
}
