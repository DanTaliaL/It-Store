using ItStore.Infrastructure;
using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Controllers
{
    public class CartController : Controller
    {
        private DataContext Data { get; set; }
        private Cart cart { get; set; }
        public CartController(DataContext DC, Cart cartService)
        {
            Data = DC;
            cart = cartService;
        }

        public IActionResult Cart(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl,
            });
        }
        public RedirectToActionResult AddToCart(int Id, string returnUrl)
        {
            Product product = Data.Products.FirstOrDefault(q => q.Id == Id);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Cart", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int Id, string returnUrl)
        {
            Product product = Data.Products.FirstOrDefault(q => q.Id == Id);
            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Cart", new { returnUrl });
        }
    }
}
