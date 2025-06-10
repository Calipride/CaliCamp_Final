using LiteDB;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly LiteDatabase _db;
        private const string _PAYMENTS = "Payments";

        public PaymentRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Payment payment)
        {
            try
            {
                if (payment == null) throw new ArgumentNullException(nameof(payment));
                _db.GetCollection<Payment>(_PAYMENTS).Insert(payment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting payment: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            try
            {
                return _db.GetCollection<Payment>(_PAYMENTS).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payments: {ex.Message}");
                throw;
            }
        }

        public Payment GetById(int id)
        {
            try
            {
                var payment = _db.GetCollection<Payment>(_PAYMENTS).FindById(id);
                if (payment == null)
                {
                    throw new KeyNotFoundException($"Payment with ID {id} not found");
                }
                return payment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving payment: {ex.Message}");
                throw;
            }
        }

        public void Update(Payment payment)
        {
            try
            {
                if (payment == null) throw new ArgumentNullException(nameof(payment));
                var exists = GetById(payment.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Payment with ID {payment.Id} not found");
                }
                _db.GetCollection<Payment>(_PAYMENTS).Update(payment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating payment: {ex.Message}");
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
                    throw new KeyNotFoundException($"Payment with ID {id} not found");
                }
                _db.GetCollection<Payment>(_PAYMENTS).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting payment: {ex.Message}");
                throw;
            }
        }
    }
}

