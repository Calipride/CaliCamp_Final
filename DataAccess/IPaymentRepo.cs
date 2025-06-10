using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public interface IPaymentRepo 
    {
        void Insert(Payment payment);
        IEnumerable<Payment> GetAll();
        Payment GetById(int id);
        void Update(Payment payment);
        void Delete(int id);
    }
}
