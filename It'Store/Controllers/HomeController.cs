using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ItStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ItStore.Models.DataFolder;

namespace ItStore.Controllers
{
    public class HomeController : Controller
    {
        private DataContext Data { get; set; }
        public HomeController(DataContext DC)
        {
            Data = DC;
        }
        public int PageSize = 8;
        public IActionResult Index(string? category, string? productmodel, int productPage = 1)
        {     
            ViewBag.SelectedCategory = category;
            var result = new ProductsListViewModel
            {
                Products = Data.Products
                .Where(q => productmodel == null || q.Model == productmodel)
                .Where(q => category == null || q.Categories == category)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                CurrentCategory = category,
                ProductModel = productmodel
            };
            return View(result);
        }
        public IActionResult Privacy() => View();

    }
}
