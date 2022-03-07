
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ItStore.Models;
using ItStore.Models.DataFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItStore.Controllers
{
    public class CustomerController : Controller
    {
        private DataContext Data { get; set; }
        public CustomerController(DataContext DC) => Data = DC; 

        [HttpPost]
        public IActionResult Customer ( Customer customer)
        {
            Data.Customers.Add(customer);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Customer()
        {
            IQueryable<Customer> customers = Data.Customers
                .Include(q => q.Requests);
            return View(customers.OrderBy(q => q.Id));
        }
        public IActionResult CustomerForm()=> View();
    }
}
