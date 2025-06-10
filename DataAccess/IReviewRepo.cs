using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public interface IReviewRepo 
    {
        void Insert(Review review);
        IEnumerable<Review> GetAll();
        Review GetById(int id);
        void Update(Review review);
        void Delete(int id);
    }
}
