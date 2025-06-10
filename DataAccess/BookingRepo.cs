using CaliCamp.Models;
using LiteDB;
using Microsoft.AspNetCore.Mvc;

namespace CaliCamp.DataAccess
{
    public class BookingRepo : IBookingRepo
    {
        private readonly LiteDatabase _db;
        private const string _BOOKINGS = "Bookings";
        private const string _CAMPINGSPOTS = "CampingSpots";
        private const string _USERS = "Users";

        public BookingRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Booking booking)
        {
            try
            {
                if (booking == null) throw new ArgumentNullException(nameof(booking));
                
                var bookingToSave = new Booking
                {
                    Id = booking.Id,
                    CampingSpotId = booking.CampingSpotId,
                    UserId = booking.UserId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    NumGuests = booking.NumGuests,
                    TotalPrice = booking.TotalPrice
                };

                _db.GetCollection<Booking>(_BOOKINGS).Insert(bookingToSave);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting booking: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            try
            {
                var bookings = _db.GetCollection<Booking>(_BOOKINGS).FindAll().ToList();
                var campingSpots = _db.GetCollection<CampingSpot>(_CAMPINGSPOTS);
                var users = _db.GetCollection<User>(_USERS);

                foreach (var booking in bookings)
                {
                    booking.CampingSpot = campingSpots.FindById(booking.CampingSpotId);
                    booking.User = users.FindById(booking.UserId);
                }

                return bookings;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving bookings: {ex.Message}");
                throw;
            }
        }

        public Booking GetById(int id)
        {
            try
            {
                var booking = _db.GetCollection<Booking>(_BOOKINGS).FindById(id);
                if (booking == null)
                {
                    throw new KeyNotFoundException($"Booking with ID {id} not found");
                }

                // Load related entities
                booking.CampingSpot = _db.GetCollection<CampingSpot>(_CAMPINGSPOTS).FindById(booking.CampingSpotId);
                booking.User = _db.GetCollection<User>(_USERS).FindById(booking.UserId);

                return booking;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving booking: {ex.Message}");
                throw;
            }
        }

        public void Update(Booking booking)
        {
            try
            {
                if (booking == null) throw new ArgumentNullException(nameof(booking));
                
                var exists = _db.GetCollection<Booking>(_BOOKINGS).FindById(booking.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Booking with ID {booking.Id} not found");
                }

                var bookingToUpdate = new Booking
                {
                    Id = booking.Id,
                    CampingSpotId = booking.CampingSpotId,
                    UserId = booking.UserId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    NumGuests = booking.NumGuests,
                    TotalPrice = booking.TotalPrice
                };

                _db.GetCollection<Booking>(_BOOKINGS).Update(bookingToUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating booking: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                var exists = _db.GetCollection<Booking>(_BOOKINGS).FindById(id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Booking with ID {id} not found");
                }
                _db.GetCollection<Booking>(_BOOKINGS).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting booking: {ex.Message}");
                throw;
            }
        }
    }
}
