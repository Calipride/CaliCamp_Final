using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class BookingRepo : IBookingRepo
    {
        readonly LiteDatabase db = new LiteDatabase(@"data.db");
        private const string _BOOKINGS = "Bookings";

        public void Insert(Booking booking)
        {
            db.GetCollection<Booking>(_BOOKINGS).Insert(booking);
        }

        public IEnumerable<Booking> GetAll()
        {
            return db.GetCollection<Booking>(_BOOKINGS).FindAll();
        }

        public Booking GetById(int id)
        {
            return db.GetCollection<Booking>(_BOOKINGS).FindById(id);
        }

        public void Update(Booking booking)
        {
            db.GetCollection<Booking>(_BOOKINGS).Update(booking);
        }

        public void Delete(int id)
        {
            db.GetCollection<Booking>(_BOOKINGS).Delete(id);
        }



    }
}
