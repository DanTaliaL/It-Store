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

        public IActionResult Catalog(string category, string searchstring, string productmodel, bool HotProduct, int productPage = 1)
        {
            ViewBag.SearchingString = searchstring;
            ViewBag.SelectedCategory = category;
            ViewBag.ProductModel = productmodel;

            if (HotProduct)
            {

                double MostBigPrice = 1.0;
                foreach (var q in Data.Products.OrderBy(a => a.Price))
                {
                    MostBigPrice = q.Price;
                }

                double MostSmallPrice = 1.0;
                foreach (var q in Data.Products.OrderByDescending(q => q.Price))
                {
                    MostSmallPrice = q.Price;
                }

                double SecondPoint = MostBigPrice * 0.35;
                double ThirdPoint = MostBigPrice * 0.65;

                var result = new ProductsListViewModel
                {

                    Products = Data.Products
               .Where(q => (q.Price - MostSmallPrice) / (SecondPoint - MostSmallPrice) > 0.75 && (ThirdPoint - q.Price) / (MostBigPrice - ThirdPoint) + 1 > 0.75)
               .Where(q => productmodel == null || q.Model == productmodel)
               .Where(q => category == null || q.Categories == category)
               .OrderBy(p => p.Id)
               .Skip((productPage - 1) * PageSize)
               .Take(PageSize),
                    PaginInfo = new PaginInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = Data.Products.Count()
                    },
                    CurrentCategory = category,
                    ProductModel = productmodel,
                    Searchstring = searchstring
                };

                if (HotProduct)
                {
                    result.PaginInfo = new PaginInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage= PageSize,
                        TotalItems = result.Products.Count()
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
            else
            {
                var result = new ProductsListViewModel
                {
                    Products = Data.Products
                .Where(q => productmodel == null || q.Model == productmodel)
                .Where(q => category == null || q.Categories == category)
                .Where(q => searchstring == null || q.Name.Contains(searchstring) || q.Model.Contains(searchstring) || q.Price.ToString() == searchstring)
                .OrderBy(p => p.Id)
                .Skip((productPage - 1) * PageSize)
                .Take(PageSize),
                    PaginInfo = new PaginInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = Data.Products.Count()
                    },
                    CurrentCategory = category,
                    ProductModel = productmodel,
                    Searchstring = searchstring
                };

                if (searchstring != null)
                {
                    result.PaginInfo = new PaginInfo
                    {
                        CurrentPage = productPage,
                        ItemsPerPage = PageSize,
                        TotalItems = Data.Products.Where(q => q.Name.Contains(searchstring) || q.Model.Contains(searchstring)).Count()
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


        }

        public IActionResult ProductCart(int ProdId, string? ProdName)
        {
            ViewBag.Id = ProdId;
            ViewBag.Name = ProdName;
            var Result = new ProductCartViewModel
            {

                Products = Data.Products.Where(q => q.Id == ProdId),
                Pictures = Data.Pictures.Where(q => q.Name == ProdName),
                Options = Data.Options.Where(q => q.ProductId == ProdId),
                ProductID = ProdId,
                Commentaries = Data.Comments.Where(q => q.ProductID == ProdId).OrderByDescending(q => q.Id),
                UserName = User.Identity.Name,
            };
            return View(Result);
        }
    }
}
