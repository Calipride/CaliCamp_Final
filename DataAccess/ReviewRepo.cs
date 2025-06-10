using LiteDB;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public class ReviewRepo : IReviewRepo
    {
        private readonly LiteDatabase _db;
        private const string _REVIEWS = "Reviews";

        public ReviewRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Review review)
        {
            try
            {
                if (review == null) throw new ArgumentNullException(nameof(review));
                _db.GetCollection<Review>(_REVIEWS).Insert(review);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting review: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Review> GetAll()
        {
            try
            {
                return _db.GetCollection<Review>(_REVIEWS).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving reviews: {ex.Message}");
                throw;
            }
        }

        public Review GetById(int id)
        {
            try
            {
                var review = _db.GetCollection<Review>(_REVIEWS).FindById(id);
                if (review == null)
                {
                    throw new KeyNotFoundException($"Review with ID {id} not found");
                }
                return review;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving review: {ex.Message}");
                throw;
            }
        }

        public void Update(Review review)
        {
            try
            {
                if (review == null) throw new ArgumentNullException(nameof(review));
                var exists = GetById(review.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Review with ID {review.Id} not found");
                }
                _db.GetCollection<Review>(_REVIEWS).Update(review);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating review: {ex.Message}");
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
                    throw new KeyNotFoundException($"Review with ID {id} not found");
                }
                _db.GetCollection<Review>(_REVIEWS).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting review: {ex.Message}");
                throw;
            }
        }
    }
}

