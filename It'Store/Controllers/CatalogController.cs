using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Controllers
{
    public class CatalogController : Controller
    {
        private DataContext Data { get; set; }
        public CatalogController(DataContext DC)
        {
            Data = DC;
        }
        public int PageSize = 4;
        public IActionResult Catalog(string category, int productPage = 1)
        {
            ViewBag.SelectedCategory = category;
            return View(new ProductsListViewModel
            {
                Products = Data.Products
                .Where(q => category == null || q.Categories == category)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null?
                    Data.Products.Count():
                    Data.Products.Where(q=>q.Categories==category).Count()
                },
                CurrentCategory = category
            });
        }
        [HttpPost]
        public IActionResult Catalog(string searchstring, int productPage = 1, int Debager =1)//debager кастыльная заглушка
        {
            ViewBag.SearchingString = searchstring;
            return View(new ProductsListViewModel
            {
                Products = Data.Products
                .Where(q => q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.SEO.Contains(searchstring) || q.Price.ToString() == searchstring)
                .OrderBy(p => p.Price)
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
    }
}
