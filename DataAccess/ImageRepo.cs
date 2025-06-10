using LiteDB;
using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public class ImageRepo : IImageRepo
    {
        LiteDatabase db = new LiteDatabase(@"data.db");
        private const string _IMAGES = "Images";

        public void Insert(Image image)
        {
            db.GetCollection<Image>(_IMAGES).Insert(image);
        }

        public IEnumerable<Image> GetAll()
        {
            return db.GetCollection<Image>(_IMAGES).FindAll();
        }

        public Image GetById(int id)
        {
            return db.GetCollection<Image>(_IMAGES).FindById(id);
        }

        public void Update(Image image)
        {
            db.GetCollection<Image>(_IMAGES).Update(image);
        }

        public void Delete(int id)
        {
            db.GetCollection<Image>(_IMAGES).Delete(id);
        }

    }
}
