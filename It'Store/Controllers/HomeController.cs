using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ItStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Identity;

namespace ItStore.Controllers
{
    public class HomeController : Controller
    {
        private DataContext Data { get; set; }
        private UserManager<AppUser> UserManager { get; set; }
        public HomeController(DataContext DC, UserManager<AppUser> userManager)
        {
            Data = DC;
            this.UserManager = userManager;
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
                .OrderByDescending(q=>q.Price)
                .Take(PageSize),
                CurrentCategory = category,
                ProductModel = productmodel
            };
            return View(result);
        }
        public IActionResult Privacy() => View();

        [HttpPost]
        public IActionResult Feedback(string EMail, string Text, string TypeMesseage, bool TypeFeedback, string? ProductName)
        {
            if (TypeFeedback)
            {

                if (User.Identity.IsAuthenticated)
                {
                    AppUser appUser = UserManager.Users.FirstOrDefault(q => q.UserName == User.Identity.Name);
                    FeedBack feedBack = new FeedBack
                    {
                        TypeMessage = TypeMesseage,
                        ProductName = ProductName,
                        TypeFeedback = TypeFeedback,
                        Text = Text,
                        Login = User.Identity.Name,
                        Created = DateTime.Now,
                        Closed = DateTime.Now,
                        FeedbakStatus = true,
                        MainEmail = appUser.Email,

                    };

                    if (EMail != null)
                    {
                        feedBack.AuxiliaryEmail = EMail;
                    }

                    Data.Add(feedBack);
                    Data.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login","Account");
                }              
            }
            else
            {
                AppUser appUser = UserManager.Users.FirstOrDefault(q => q.UserName == User.Identity.Name);
                FeedBack feedBack = new FeedBack
                {
                    TypeMessage = TypeMesseage,
                    Text = Text,
                    Login = User.Identity.Name,
                    Created = DateTime.Now,
                    Closed = DateTime.Now,
                    FeedbakStatus = true,

                };
                if (feedBack.MainEmail == null)
                {
                    feedBack.MainEmail = "Не зарегестрирован";
                    feedBack.Login = "Гость";
                }
                else
                {
                    feedBack.MainEmail = appUser.Email;
                }

                if (EMail != null)
                {
                    feedBack.AuxiliaryEmail = EMail;
                }


                if (appUser != null)
                {
                    if (appUser.FatherName != null)
                    {
                        feedBack.Name = $"{appUser.LastName} {appUser.FirstName} {appUser.FatherName}";
                    }

                }

                Data.Add(feedBack);
                Data.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}
