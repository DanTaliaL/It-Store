using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using ItStore.Models.DataFolder;
using System.Linq;

namespace ItStore.Controllers
{
    public class WareHouseController : Controller
    {
        private DataContext Data { get; set; }
        public WareHouseController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult WareHouse(WareHouse wareHouse)
        {
            Data.WareHouse.Add(wareHouse);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult WareHouse()
        {
            IQueryable<WareHouse> warehouses = Data.WareHouse
                .Include(q => q.Products);
            return View(Data.WareHouse.OrderBy(q=>q.Id));
        }

        public IActionResult WareHouseForm() => View();
    }
}
