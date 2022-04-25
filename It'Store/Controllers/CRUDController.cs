using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            return RedirectToAction("Product","Product");
        }

        public IActionResult Options(int Id)
        {
            ViewBag.Id = Id;
            IQueryable<Options> options = Data.Options.Where(q=>q.ProductId==Id)
                .Include(q => q.Product);
            return View(options);
        }
        public IActionResult OptionsForm(int Id)
        {
            ViewBag.Id=Id;
            return View();
        }

        public IActionResult OptionsDelete(int Id)
        {
            Options options = Data.Options.Where(q => q.Id == Id).FirstOrDefault();
            Data.Options.Remove(options);
            Data.SaveChanges();
            return RedirectToAction("Product","Product");
        }
    }

    public class OrderController : Controller
    {
        private IOrderRepository repository { get; set; }
        private Cart cart { get; set; }
        public OrderController(IOrderRepository repository, Cart cart)
        {
            this.repository = repository;
            this.cart = cart;
        }

        [HttpPost]
        public IActionResult OrderForm(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }

            //Data.Orders.Add(order);
            //Data.SaveChanges();
            //return RedirectToAction();
        }
        public IActionResult Completed()
        {
            cart.Clear();
            return View(cart);
        }

        public IActionResult Order()
        {
            IQueryable<Order> orders = repository.Orders
                .Include(q => q.Products);
            return View(repository.Orders.OrderBy(q => q.Id));
        }

        public IActionResult OrderForm() => View(new Order());
    }

    public class ProductController : Controller
    {
        private DataContext Data { get; set; }
        public ProductController(DataContext DC) => Data = DC;
        public int PageSize = 4;

        //for Image
        private byte[] ConvertToBytes(IFormFile file)
        {
            try
            {
                Stream stream = file.OpenReadStream();
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch (NullReferenceException)
            {
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.SetLength(1);
                    return memoryStream.ToArray();
                }
            }

        }

        public IActionResult Product()
        {
            IQueryable<Product> products = Data.Products
                .Include(q => q.Categories)
                .Include(q => q.WareHouse)
                .Include(q => q.Options)
                .Include(q => q.Suppliers)
                .Include(q => q.Orders);
            return View(Data.Products.OrderBy(q=>q.Id));
        }

        [HttpPost]
        public IActionResult ProductForm(Product product, IFormFile file)
        {
            if (file != null)
            {
                byte[] ImageData = ConvertToBytes(file);
                product.Image = ImageData;
                Data.Products.Add(product);
                Data.SaveChanges();
                return RedirectToAction("Product");
            }
            return View();
        }
        public IActionResult ProductForm() => View();

        public IActionResult ProductDelete(int Id)
        {
            Product product = Data.Products.Where(q => q.Id == Id).FirstOrDefault();
            Data.Products.Remove(product);
            Data.SaveChanges();
            return RedirectToAction("Product");
        }

        [HttpPost]
        public IActionResult ProductUpdate(Product product, IFormFile file)
        {
            
            Product update = Data.Products.FirstOrDefault(q=>q.Id==product.Id);
            update.Name = product.Name;
            update.Price = product.Price;
            update.Model = product.Model;
            update.Categories= product.Categories;
            update.WareHouseId = product.WareHouseId;
            byte[] ImageData = ConvertToBytes(file);
            update.Image = ImageData;
            Data.SaveChanges();
           return RedirectToAction("Product");
        }

        public IActionResult ProductUpdate(int Id) => View(Data.Products.FirstOrDefault(q=>q.Id==Id));

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

    public class CommentariesController : Controller
    {
        private DataContext Data { get; set; }
        public CommentariesController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Commentaries(Commentaries commentaries, int ProdId, string ProdName)
        {
            commentaries.Created = DateTime.Now;
            Data.Comments.Add(commentaries);
            Data.SaveChanges();
            return RedirectToAction("ProductCart", "Catalog", new { ProdId, ProdName });
        }
    }
}
