using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Controllers
{
    public class CatalogController : Controller
    {
        private DataContext Data { get; set; }
        public CatalogController(DataContext DC) => Data = DC;
        public int PageSize = 4;
        public IActionResult Catalog(int productPage=1)
        {           
            return View(Data.Products.OrderBy(p => p.Id).Skip((productPage - 1) * PageSize).Take(PageSize));
        }
    }
}
