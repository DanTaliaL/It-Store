using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using System.Linq;

namespace ItStore.Controllers
{
    public class ProductController : Controller
    {
        private DataContext Data { get; set; }
        public ProductController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Product (Product product)
        {
            Data.Products.Add(product);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Product()
        {
            IQueryable<Product> products = Data.Products
                .Include(q => q.Categories)
                .Include(q => q.WareHouse)
                .Include(q => q.Options)
                .Include(q => q.Suppliers)
                .Include(q => q.Orders);
            return View(Data.Products.OrderBy(q=>q.Id));
        }

        public IActionResult ProductForm() => View();
    }
}
