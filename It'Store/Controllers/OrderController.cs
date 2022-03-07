using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using System.Linq;

namespace ItStore.Controllers
{
    public class OrderController : Controller
    {
        private DataContext Data { get; set; }
        public OrderController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Order(Order order)
        {
            Data.Orders.Add(order);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Order()
        {
            IQueryable<Order> orders = Data.Orders
                .Include(q => q.Products);
            return View(Data.Orders.OrderBy(q => q.Id));
        }

        public IActionResult OrderForm() => View();
    }
}
