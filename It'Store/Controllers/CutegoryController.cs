using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ItStore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ItStore.Controllers
{
    public class CutegoryController : Controller
    {
        private DataContext Data { get; set; }
        public CutegoryController (DataContext DC) => Data=DC;

        [HttpPost]
        public IActionResult Cutegory ( Category category)
        {
            Data.Category.Add(category);
            Data.SaveChanges();
            return View (category);
        }

        public IActionResult Category ()
        {
            IQueryable<Category> categories = Data.Category
                .Include(q => q.Products);
                
            return View (categories.OrderBy(q=>q.Id));
        }
        public IActionResult CategoryForm() => View();
    }
}
