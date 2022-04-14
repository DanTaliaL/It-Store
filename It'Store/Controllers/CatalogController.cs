using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItStore.Controllers
{
    public class CatalogController : Controller
    {
        private DataContext Data { get; set; }
        public CatalogController(DataContext DC)
        {
            Data = DC;
        }
        public int PageSize = 8;
         
        public IActionResult Catalog(string category, string searchstring, string productmodel, int productPage = 1)
        {
            ViewBag.SearchingString = searchstring;
            ViewBag.SelectedCategory = category;
            var result = new ProductsListViewModel
            {
                Products = Data.Products
                .Where(q=>productmodel == null || q.Model == productmodel)
                .Where(q =>category == null || q.Categories == category)
                .Where(q=>searchstring == null|| q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.SEO.Contains(searchstring) || q.Price.ToString() == searchstring)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                PaginInfo =  new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Data.Products.Count()
                },
                CurrentCategory = category,
                ProductModel =productmodel,
                Searchstring = searchstring               
            };

            if (searchstring != null)
            {
                result.PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = Data.Products.Where(q => q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.SEO.Contains(searchstring)).Count()
                };
            }
            else if (category != null)
            {
                result.PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    Data.Products.Count() :
                    Data.Products.Where(q => q.Categories == category).Count()
                };
            }
            else if (productmodel != null)
            {
                result.PaginInfo = new PaginInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = productmodel == null ?
                    Data.Products.Count() :
                    Data.Products.Where(q => q.Model == productmodel).Count()
                };
            }

            return View(result);
        }

        public IActionResult ProductCart(int Id, string? Name)
        {
            var Result = new ProductCartViewModel
            {
                Products = Data.Products.Where(q=>q.Id==Id),
                Pictures = Data.Pictures.Where(q=>q.Name==Name),
                Options = Data.Options,
                ProductId = Id,
                
            };
            return View(Result);
        }
    }
}
