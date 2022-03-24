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
        public IActionResult Catalog(int productPage = 1)
        {
            return View(new ProductsListViewModel
            {
                Products = Data.Products
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Data.Products.Count()
                }
            });
        }
        [HttpPost]
        public IActionResult Catalog(string searchstring, int productPage = 1)
        {
            var result = new ProductsListViewModel
            {
                Products = Data.Products
                .Where(q => q.Name == searchstring || q.Model == searchstring || q.SEO == searchstring || q.Price.ToString() == searchstring)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize),
                PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Data.Products.Count()
                }
            };
            return View(result); 
        }
    }
}
