using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;



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

        public IActionResult Create() => View();

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
