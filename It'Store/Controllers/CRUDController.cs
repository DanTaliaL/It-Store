using ItStore.Models.DataFolder;
using ItStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ItStore.Controllers
{
    [Authorize]
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
        private UserManager<AppUser> userManager { get; set; }
        private IOrderRepository repository { get; set; }
        private Cart cart { get; set; }
        public OrderController(IOrderRepository repository, Cart cart, DataContext DC, UserManager<AppUser> userManager)
        {
            this.repository = repository;
            this.cart = cart;
            this.userManager = userManager;
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

                    if (product != null)
                    {
                        if (product.Quantity >= q.Quantity)
                        {
                            product.Quantity -= q.Quantity;
                        }
                        else
                        {
                            ModelState.AddModelError("", $"Приносим свои извенения {q.Product.Name} {q.Product.Model} осталось всего {product.Quantity}");
                            return View(order);
                        }
                                             
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Приносим свои извенения {q.Product.Name} {q.Product.Model} на данный момент отсутствует");
                        return View(order);
                    }

                   

                }
                order.Lines = cart.Lines.ToArray();
                if (TotalPrice != null)
                {
                    order.TotalPrice = TotalPrice;
                    order.Promotions = PromotionCode;
                }


                
                foreach (var q in cart.Lines)
                {
                    var History = new History();
                    ProductQuantity product = Data.ProductsQuantity.FirstOrDefault(p => p.Product.Name == q.Product.Name);
                    Promotion promotion = Data.Promotions.FirstOrDefault(q => q.PromotionCode == PromotionCode);

                    History.ProductName = $"{q.Product.Name}  {q.Product.Model}";                  
                    History.ProductQuantity = Convert.ToString(q.Quantity);
                    History.DateTime = order.TimeOrders;
                    History.Buyer = User.Identity.Name;
                   
                    if (PromotionCode != null)
                    {
                        History.PromotionCode = PromotionCode;
                        History.NamePromotion = promotion.Name;
                        History.PromotionDescription = promotion.Description;
                        History.PercentAge = promotion.Percentage.ToString();
                        History.ProductPrice = (Convert.ToDouble(q.Product.Price)-(Convert.ToDouble(promotion.Percentage/100.0)* Convert.ToDouble(q.Product.Price))).ToString();
                    }
                    else
                    {
                        History.PromotionCode = "-";
                        History.ProductPrice = q.Product.Price.ToString();
                        History.NamePromotion = "-";
                        History.PromotionDescription = "-";
                        History.PercentAge = "-";
                    }

                    Data.Histories.Add(History);
                    Data.SaveChanges();

                }           
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
            return View(repository.Orders.OrderBy(q => q.Id));
        }

        public IActionResult OrderForm(string? PromotionStatus, decimal? TotalPrice, string? PromotionCode)
        {
            ViewBag.TotalPrice = TotalPrice;
            ViewBag.PromotionStatus = PromotionStatus;
            ViewBag.PromotionCode = PromotionCode;
            var user = userManager.Users.Where(q => q.UserName == User.Identity.Name);
            var order = new Order
            {
                City = user.Select(q => q.City).FirstOrDefault(),
                Street = user.Select(q => q.Street).FirstOrDefault(),
                House = user.Select(q => q.House).FirstOrDefault(),
                Flat = user.Select(q => q.Flat).FirstOrDefault(),
            };
            return View(order);

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
            else if (promotion.ClosedDate >= DateTime.Now)
            {
                string PromotionStatus = $"Промокод {promotion.PromotionCode} на {promotion.Percentage}% успешно применен";
                Data.SaveChanges();

                int Percent = promotion.Percentage;
                decimal TotalValue = Convert.ToDecimal(Percent / 100.0);
                decimal TotalPrice = cart.ComputeTotalValue() - (cart.ComputeTotalValue() * TotalValue);

                return RedirectToAction("OrderForm", new { PromotionStatus, TotalPrice, PromotionCode });
            }
            else
            {
                string PromotionStatus = $"Сожалеем но введеный вами промокод до {promotion.ClosedDate.ToShortDateString()}, к сожалению вы не успели";
                decimal TotalPrice = cart.ComputeTotalValue();
                return RedirectToAction("OrderForm", new { PromotionStatus, TotalPrice });

            }

        }

    }
    [Authorize]
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
    [Authorize]
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

        public IActionResult ProductQuantityForm()
        {
            var result = new ProductQuantityViewModel
            {
                Products = Data.Products.Select(q=>new Product
                {
                    Id = q.Id,
                    Name = q.Name,
                    Model = q.Model,
                }),
                WareHouse = Data.WareHouse.Select(q=>new WareHouse
                {
                    Id=q.Id,
                    Name=q.Name,
                    Adress=q.Adress,
                }),
            
            };
            return View(result);
        }

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
    [Authorize]
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
    [Authorize]
    public class PromotionController : Controller
    {
        private DataContext Data { get; set; }
        public PromotionController(DataContext DC) => Data = DC;

        [HttpPost]
        public IActionResult Promotion(Promotion promotion)
        {
            promotion.PromotionCode = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
            promotion.CreateDate = DateTime.Now;
            if (promotion.ClosedDate==null || promotion.ClosedDate <promotion.CreateDate)
            {
                promotion.ClosedDate = DateTime.Now;
            }
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
            update.PublicStatus = promotion.PublicStatus;
            update.ClosedDate = promotion.ClosedDate;
            update.PromotionCode = promotion.PromotionCode;
            Data.SaveChanges();
            return RedirectToAction("Promotion");
        }

        public IActionResult PromotionUpdate(int Id) => View(Data.Promotions.Where(q => q.Id == Id).FirstOrDefault());

    }
    [Authorize]
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
