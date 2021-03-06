using ItStore.Models;
using ItStore.Models.DataFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ItStore.Controllers
{
    public class CustomerController : Controller
    {
        private DataContext Data { get; set; }
        public CustomerController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Customer(Customer customer)
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
        public IActionResult CustomerForm() => View();
    }

    public class CutegoryController : Controller
    {
        private DataContext Data { get; set; }
        public CutegoryController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Cutegory(Category category)
        {
            Data.Category.Add(category);
            Data.SaveChanges();
            return View(category);
        }

        public IActionResult Category()
        {
            IQueryable<Category> categories = Data.Category
                .Include(q => q.Products);

            return View(categories.OrderBy(q => q.Id));
        }
        public IActionResult CategoryForm() => View();
    }

    public class ManufacturerController : Controller
    {
        private DataContext Data { get; set; }
        public ManufacturerController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Manufacturer(Manufacturer manufacturer)
        {
            Data.Manufacturer.Add(manufacturer);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Manufacturer()
        {
            IQueryable<Manufacturer> manufacturers = Data.Manufacturer
                .Include(q => q.Suppliers);
            return View(manufacturers.OrderBy(q => q.Id));
        }
        public IActionResult ManufacturerForm() => View();
    }

    public class OptionsController : Controller
    {
        private DataContext Data { get; set; }
        public OptionsController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Options(Options options)
        {
            Data.Options.Add(options);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Options()
        {
            IQueryable<Options> options = Data.Options
                .Include(q => q.Product);
            return View(Data.Options.OrderBy(q => q.Id));
        }
        public IActionResult OptionsForm() => View();
    }

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

    public class ProductController : Controller
    {
        private DataContext Data { get; set; }
        public ProductController(DataContext DC) => Data = DC;    

        [HttpPost]
        public IActionResult Product(Product product)
        {
            Data.Products.Add(product);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Product()
        {
            IQueryable<Product> products = Data.Products
                .Include(q => q.Categories)
                .Include(q => q.WareHouse)
                .Include(q => q.Options)
                .Include(q => q.Suppliers)
                .Include(q => q.Orders);
            return View(Data.Products.OrderBy(q => q.Id));
        }

        public IActionResult ProductForm() => View();
    }

    public class RequestController : Controller
    {
        private DataContext Data { get; set; }
        public RequestController(DataContext DC) => Data = DC;

        [HttpPost]
        public new IActionResult Request(Request request)
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
            return View(Data.Requests.OrderBy(q => q.Id));
        }
        public IActionResult RequestForm() => View();
    }

    public class SupplierController : Controller
    {
        private DataContext Data { get; set; }
        public SupplierController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Supplier(Supplier supplier)
        {
            Data.Suppliers.Add(supplier);
            Data.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult Supplier()
        {
            IQueryable<Supplier> suppliers = Data.Suppliers
                .Include(q => q.Requests)
                .Include(q => q.Product)
                .Include(q => q.Manufacturers);
            return View(Data.Suppliers.OrderBy(q => q.Id));
        }

        public IActionResult SupplierForm() => View();
    }

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
            return View(Data.WareHouse.OrderBy(q => q.Id));
        }

        public IActionResult WareHouseForm() => View();
    }
}
