using Microsoft.AspNetCore.Mvc;
using CaliCamp.Models;

namespace CaliCamp.DataAccess
{
    public interface IImageRepo 
    {
        void Insert(Image image);
        IEnumerable<Image> GetAll();
        Image GetById(int id);
        void Update(Image image);
        void Delete(int id);

    }
}
