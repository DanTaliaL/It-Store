using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItStore.Controllers
{
    public class PictureController : Controller
    {
        private DataContext Data { get; set; }
        public PictureController(DataContext DC) => Data = DC;

        public IActionResult Picture()
        {
            return View(Data.Pictures);
        }

        public IActionResult Create()
        {
            var result = new PictureViewModel
            {
                Products = Data.Products.Select(q=>new Product
                {
                    Name = q.Name,
                    Model = q.Model,
                    Image= q.Image,
                }),
            };
            return View(result);
        }

        
        public IActionResult PictureDelete(int id)
        {
            Picture picture = Data.Pictures.FirstOrDefault(q=>q.Id == id);
            Data.Pictures.Remove(picture);
            Data.SaveChanges();
            return RedirectToAction("Picture");
        }

        //---
        private byte[] ConvertToBytes(IFormFile file)
        {
            Stream stream = file.OpenReadStream();
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [HttpPost]
        public IActionResult Create(Picture picture, IFormFile uploadImage)
        {
            if (uploadImage != null)
            {
                byte[] ImageData = ConvertToBytes(uploadImage);
                picture.Image = ImageData;
                Data.Pictures.Add(picture);
                Data.SaveChanges();
                return RedirectToAction("Picture");
            }
            return View(picture);
        }
    }
}
