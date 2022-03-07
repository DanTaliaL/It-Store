using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using System.Linq;

namespace ItStore.Controllers
{
    public class OptionsController : Controller
    {
        private DataContext Data { get; set; }
        public OptionsController(DataContext DC) => Data = DC;
        
        [HttpPost]
        public IActionResult Options (Options options)
        {
            Data.Options.Add(options);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Options ()
        {
            IQueryable<Options> options = Data.Options
                .Include(q => q.Product);
            return View(Data.Options.OrderBy(q=>q.Id));
        }
        public IActionResult OptionsForm() => View();
    }
}
