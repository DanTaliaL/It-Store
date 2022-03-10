using ItStore.Infrastructure;
using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;

namespace ItStore.Controllers
{
    public class CartController : Controller
    {
        private DataContext Data { get; set; }
        public CartController(DataContext DC) => Data = DC;
        public RedirectToActionResult AddToCart(int Id, string returnUrl)
        {
            Product product = Data.Products.FirstOrDefault(q => q.Id == Id);
            if (product != null)
            {
                Cart cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }
            return RedirectToAction("Cart", new {returnUrl});
        }
        private Cart GetCart()
        {
            Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
            return cart;
        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        public IActionResult Cart(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
    }
}
