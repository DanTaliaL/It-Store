using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace ItStore.Controllers
{

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
        public IActionResult Options(Options options, int ProdId)
        {
            Data.Options.Add(options);
            Data.SaveChanges();
            return RedirectToAction("Options", new { ProdId });
        }

        public IActionResult Options(int ProdId)
        {
            ViewBag.ProdId = ProdId;
            IQueryable<Options> options = Data.Options.Where(q => q.ProductId == ProdId)
                .Include(q => q.Product);
            return View(options);
        }
        public IActionResult OptionsForm(int ProdId)
        {
            ViewBag.ProdId = ProdId;
            return View();
        }

        public IActionResult OptionsDelete(int ProdId)
        {
            Options options = Data.Options.Where(q => q.ProductId == ProdId).FirstOrDefault();
            Data.Options.Remove(options);
            Data.SaveChanges();
            return RedirectToAction("Options", new { ProdId });
        }

        [HttpPost]
        public IActionResult OptionsUpdate(Options options, int ProdId)
        {
            Options update = Data.Options.Where(q => q.ProductId == ProdId).FirstOrDefault();

            update.ProductId = ProdId;

            update.ProcessorModelName = options.ProcessorModelName;
            update.QuantityCore = options.QuantityCore;
            update.NumberOfThreads = options.NumberOfThreads;
            update.CPUFrequency = options.CPUFrequency;
            update.MaxCPUFrequency = options.MaxCPUFrequency;

            update.Model = options.Model;
            update.ManufacturerCode = options.ManufacturerCode;
            update.ReleaseYear = options.ReleaseYear;
            update.OperatingSystem = options.OperatingSystem;

            update.CoverMaterial = options.CoverMaterial;
            update.HousingMaterial = options.HousingMaterial;

            update.ScreenType = options.ScreenType;
            update.ScreenDiagonal = options.ScreenDiagonal;
            update.ScreenResolution = options.ScreenResolution;
            update.MaximumScreenRefreshRate = options.MaximumScreenRefreshRate;
            update.PixelDensity = options.PixelDensity;

            update.RAMType = options.RAMType;
            update.RAMMemory = options.RAMMemory;
            update.RAMFrequency = options.RAMFrequency;

            update.TypeOfGraphicsAccelerator = options.TypeOfGraphicsAccelerator;
            update.BuiltInGraphicsCardModel = options.BuiltInGraphicsCardModel;
            update.DiscreteGraphicsCardModel = options.DiscreteGraphicsCardModel;

            update.VolumeSSD = options.VolumeSSD;
            update.VolumeHDD = options.VolumeHDD;
            update.VolumeEMMC = options.VolumeEMMC;
            Data.SaveChanges();
            return RedirectToAction("Options", new { ProdId });
        }

        public IActionResult OptionsUpdate(int ProdId)
        {
            ViewBag.ProdId = ProdId;
            return View(Data.Options.Include(q => q.Product).Where(q => q.ProductId == ProdId && q.Product.Id == ProdId).FirstOrDefault());
        }
    }

    public class OrderController : Controller
    {
        private DataContext Data { get; set; }
        private IOrderRepository repository { get; set; }
        private Cart cart { get; set; }
        public OrderController(IOrderRepository repository, Cart cart, DataContext DC)
        {
            this.repository = repository;
            this.cart = cart;
            Data = DC;
        }

        [HttpPost]
        public IActionResult OrderForm(Order order, decimal TotalPrice, string? PromotionCode)
        {                      
            order.Name = User.Identity.Name;
            order.TotalPrice = cart.ComputeTotalValue();
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста");
            }
            if (ModelState.IsValid)
            {
                foreach (var q in cart.Lines)
                {
                    ProductQuantity product = Data.ProductsQuantity.FirstOrDefault(p => p.Product.Name == q.Product.Name);
                    if (product.Quantity >= q.Quantity)
                    {
                        product.Quantity -= q.Quantity;                       
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Приносим свои извенения,{q.Product.Name} осталось всего {product.Quantity}");
                        return View(order);
                    }
                }
                order.Lines = cart.Lines.ToArray();
                if (TotalPrice != null)
                {
                    order.TotalPrice = TotalPrice;
                    order.Promotions = PromotionCode;
                }
                Data.SaveChanges();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }
        public IActionResult Completed(string PromotionStatus)
        {
            ViewBag.PromotionStatus = PromotionStatus;
            cart.Clear();
            return View(cart);
        }

        public IActionResult Order()
        {
            IQueryable<Order> orders = repository.Orders
                .Include(q => q.Products);
            return View(repository.Orders.OrderBy(q => q.Id));
        }

        public IActionResult OrderForm(string? PromotionStatus, decimal? TotalPrice, string? PromotionCode)
        {
            ViewBag.TotalPrice = TotalPrice;
            ViewBag.PromotionStatus = PromotionStatus;
            ViewBag.PromotionCode = PromotionCode;
            return View();

        }

        [HttpPost]
        public IActionResult PromotionRemains(string PromotionCode)
        {
            Promotion promotion = Data.Promotions.Where(q => q.PromotionCode == PromotionCode).FirstOrDefault();

            if (promotion == null)
            {
                string PromotionStatus = "Промокод не найден, обратитесь в поддержку.";
                decimal TotalPrice = cart.ComputeTotalValue();
                return RedirectToAction("OrderForm", new { PromotionStatus, TotalPrice });
            }
            else if (promotion.Quantity >= 1)
            {
                promotion.Quantity -= 1;
                string PromotionStatus = $"Промокод на {promotion.Percentage}% успешно применен";
                Data.SaveChanges();

                int Percent = promotion.Percentage;
                decimal TotalValue = Convert.ToDecimal(Percent / 100.0);
                decimal TotalPrice = cart.ComputeTotalValue() - (cart.ComputeTotalValue() * TotalValue);

                return RedirectToAction("OrderForm", new { PromotionStatus, TotalPrice, PromotionCode });
            }
            else
            {
                string PromotionStatus = "Количество промокодов ограничено, к сожалению вы не успели";
                decimal TotalPrice = cart.ComputeTotalValue();
                return RedirectToAction("OrderForm", new { PromotionStatus, TotalPrice });

            }

        }

    }

    public class ProductController : Controller
    {
        private DataContext Data { get; set; }
        public ProductController(DataContext DC) => Data = DC;

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

        public IActionResult Product(int? Id)
        {
            if (Id != null)
            {
                return View(Data.Products.Where(q => q.Id == Id));
            }
            else
            {
                IQueryable<Product> products = Data.Products
                  .Include(q => q.Categories)
                  .Include(q => q.Options)
                  .Include(q => q.Suppliers)
                  .Include(q => q.Orders)
                  .Include(q => q.ProductQuantity);
                return View(Data.Products.OrderBy(q => q.Id));
            }

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

            Product update = Data.Products.FirstOrDefault(q => q.Id == product.Id);
            update.Name = product.Name;
            update.Price = product.Price;
            update.Model = product.Model;
            update.Categories = product.Categories;
            byte[] ImageData = ConvertToBytes(file);
            update.Image = ImageData;
            Data.SaveChanges();
            return RedirectToAction("Product");
        }

        public IActionResult ProductUpdate(int Id) => View(Data.Products.FirstOrDefault(q => q.Id == Id));

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

    public class ProductQuantityController : Controller
    {
        private DataContext Data { get; set; }
        public ProductQuantityController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult ProductQuantity(ProductQuantity productQuantity)
        {
            Data.ProductsQuantity.Add(productQuantity);
            Data.SaveChanges();
            return RedirectToAction();
        }
        public IActionResult ProductQuantity(int? ProdId)
        {
            if (ProdId != null)
            {
                IQueryable<ProductQuantity> products = Data.ProductsQuantity.Where(products => products.ProductId == ProdId)
                    .Include(q => q.Product)
                    .Include(q => q.WareHouse);
                return View(products);
            }
            else
            {
                IQueryable<ProductQuantity> products = Data.ProductsQuantity
                  .Include(q => q.WareHouse)
                  .Include(q => q.Product);
                return View(products.OrderBy(q => q.Id));
            }

        }

        public IActionResult ProductQuantityDelete(int Id)
        {
            ProductQuantity productQuantity = Data.ProductsQuantity.FirstOrDefault(q => q.Id == Id);
            Data.ProductsQuantity.Remove(productQuantity);
            Data.SaveChanges();
            return RedirectToAction("ProductQuantity");
        }

        public IActionResult ProductQuantityForm() => View();

        [HttpPost]
        public IActionResult ProductQuantityUpdate(ProductQuantity productQuantity, int Id)
        {
            ProductQuantity update = Data.ProductsQuantity.FirstOrDefault(q => q.Id == Id);
            update.Quantity = productQuantity.Quantity;
            update.ProductId = productQuantity.ProductId;
            Data.SaveChanges();
            return RedirectToAction("ProductQuantity");
        }
        public IActionResult ProductQuantityUpdate(int Id)
        {
            ViewBag.Id = Id;
          return View(Data.ProductsQuantity.FirstOrDefault(q => q.Id == Id));
        }
        
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
            IQueryable<ProductQuantity> products = Data.ProductsQuantity
                .Include(q => q.WareHouse)
                .Include(q => q.Product);

            return View(products.OrderBy(q => q.Id));
        }

        public IActionResult WareHouseForm() => View();
    }
    public class PromotionController : Controller
    {
        private DataContext Data { get; set; }
        public PromotionController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Promotion(Promotion promotion)
        {
            promotion.PromotionCode = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            Data.Promotions.Add(promotion);
            Data.SaveChanges();
            return RedirectToAction();
        }
        public IActionResult Promotion() => View(Data.Promotions);
        public IActionResult PromotionForm() => View();

        public IActionResult PromotionDelete(int Id)
        {
            Promotion promotion = Data.Promotions.Where(q => q.Id == Id).FirstOrDefault();
            Data.Promotions.Remove(promotion);
            Data.SaveChanges();
            return RedirectToAction("Promotion");
        }

        [HttpPost]
        public IActionResult PromotionUpdate(Promotion promotion, int Id)
        {
            Promotion update = Data.Promotions.Where(q => q.Id == Id).FirstOrDefault();
            update.Percentage = promotion.Percentage;
            update.Description = promotion.Description;
            update.Name = promotion.Name;
            update.Quantity = promotion.Quantity;
            update.PromotionCode = promotion.PromotionCode;
            Data.SaveChanges();
            return RedirectToAction("Promotion");
        }

        public IActionResult PromotionUpdate(int Id) => View(Data.Promotions.Where(q => q.Id == Id).FirstOrDefault());

    }

    public class CommentariesController : Controller
    {
        private DataContext Data { get; set; }
        public CommentariesController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Commentaries(Commentaries commentaries, int ProdId, string ProdName, bool Grade)
        {
            commentaries.Grade = Grade;
            commentaries.Created = DateTime.Now;
            Data.Comments.Add(commentaries);
            Data.SaveChanges();
            return RedirectToAction("ProductCart", "Catalog", new { ProdId, ProdName });
        }
    }
}
