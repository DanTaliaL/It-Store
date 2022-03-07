using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using ItStore.Models.DataFolder;
using System.Linq;

namespace ItStore.Controllers
{
    public class RequestController : Controller
    {
        private DataContext Data {get; set;}
        public RequestController(DataContext DC) => Data = DC;

        [HttpPost]
        public new IActionResult Request (Request request)
        {
            Data.Requests.Add(request);
            Data.SaveChanges();
            return RedirectToAction();
        }
        public new IActionResult Request()
        {
            IQueryable<Request> query = Data.Requests
                .Include(q => q.Customer)
                .Include(q => q.Suppliers);
            return View(Data.Requests.OrderBy(q=>q.Id));
        }
        public IActionResult RequestForm() => View();
    }
}
