using LiteDB;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public class ImageRepo : IImageRepo
    {
        private readonly LiteDatabase _db;
        private const string _IMAGES = "Images";

        public ImageRepo(string connectionString = "Filename=MyData.db; Connection=shared")
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

        public void Insert(Image image)
        {
            try
            {
                if (image == null) throw new ArgumentNullException(nameof(image));
                _db.GetCollection<Image>(_IMAGES).Insert(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting image: {ex.Message}");
                throw;
            }
        }

        public IEnumerable<Image> GetAll()
        {
            try
            {
                return _db.GetCollection<Image>(_IMAGES).FindAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving images: {ex.Message}");
                throw;
            }
        }

        public Image GetById(int id)
        {
            try
            {
                var image = _db.GetCollection<Image>(_IMAGES).FindById(id);
                if (image == null)
                {
                    throw new KeyNotFoundException($"Image with ID {id} not found");
                }
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving image: {ex.Message}");
                throw;
            }
        }

        public void Update(Image image)
        {
            try
            {
                if (image == null) throw new ArgumentNullException(nameof(image));
                var exists = GetById(image.Id) != null;
                if (!exists)
                {
                    throw new KeyNotFoundException($"Image with ID {image.Id} not found");
                }
                _db.GetCollection<Image>(_IMAGES).Update(image);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating image: {ex.Message}");
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
                    throw new KeyNotFoundException($"Image with ID {id} not found");
                }
                _db.GetCollection<Image>(_IMAGES).Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting image: {ex.Message}");
                throw;
            }
        }
    }
}
