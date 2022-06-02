using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ItStore.Models.SystemsFolder;
using ItStore.Models;
using System.Threading.Tasks;
using ItStore.Models.DataFolder;
using Microsoft.EntityFrameworkCore;

namespace ItStore.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private DataContext Data;
        private Cart cart;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, DataContext Data, Cart cart)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.Data = Data;
            this.cart = cart;
        }

        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return RedirectToAction(ReturnUrl ?? "Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? ReturnUrl)
        {
            if (model.Login == null || model.Password == null)
            {
                ModelState.AddModelError(nameof(CreateUserModel.Password), "");
            }
            bool AdminStatus = false;
            if (ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(model.Login);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        AdminStatus = await userManager.IsInRoleAsync(user, "Admin");

                        if (AdminStatus)
                        {
                            return RedirectToAction(ReturnUrl ?? "AdminProfile");
                        }
                        else
                        {
                            return RedirectToAction(ReturnUrl ?? "Profile");
                        }

                    }
                }
                ModelState.AddModelError(nameof(CreateUserModel.Password), "Неверный логин или пароль");
            }
            return View(model);
        }

        public IActionResult UserCreate() => View();

        [HttpPost]
        public async Task<IActionResult> UserCreate(CreateUserModel model, string RepeatPassword)
        {
            if (model.Password == RepeatPassword)
            {
                if (ModelState.IsValid)
                {

                    AppUser user = new AppUser
                    {
                        UserName = model.Login,
                        Email = model.Email
                    };
                    IdentityResult result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                return View(model);
            }
            else
            {
               ModelState.AddModelError("", "Пароли не совпадают");
               return View();
            }
        }

        //public IActionResult UserUpdate(AppUser appUser, string UserName)
        //{
        //    AppUser update = userManager.Users.FirstOrDefault(q=>q.UserName ==UserName);
        //    return View(update);
        //}

        //[HttpPost]
        //public async Task<IActionResult> UserUpdate(CreateUserModel model, string UserName, string Password)
        //{
        //    AppUser update = userManager.Users.FirstOrDefault(q => q.UserName == UserName);

            
        //    return RedirectToAction("Profile");
        //}

        [Authorize]
        public IActionResult Index() => View(GetData(nameof(Index)));
        [Authorize(Roles = "Users")]
        public IActionResult OtherAction() => View("Index", GetData(nameof(OtherAction)));
        public Dictionary<string, object> GetData(string actionName) => new Dictionary<string, object>
        {
            ["Action"] = actionName,
            ["User"] = HttpContext.User.Identity.Name,
            ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
            ["Auth Type"] = HttpContext.User.Identity.AuthenticationType,
            ["In Users Role"] = HttpContext.User.IsInRole("Users")
        };

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Profile()
        {
            var result = new ProfileViewModel
            {
                Histories = Data.Histories.Where(q => q.Buyer == User.Identity.Name),
                Promotion = Data.Promotions.Where(q => q.PublicStatus==true),
                AppUser = Data.Users.Where(q => q.UserName == User.Identity.Name),
                ProductQuantity = Data.ProductsQuantity,
                Cart = cart
            };
            return View(result);
        }

        public IActionResult ProfileUpdate(string ProfileName) => View(userManager.Users.Where(q => q.UserName == ProfileName).FirstOrDefault());

        [Authorize]
        [HttpPost]
        public IActionResult ProfileUpdate(AppUser user, string ProfileName, string ProfileRole)
        {
            AppUser update = userManager.Users.Where(q => q.UserName == ProfileName).FirstOrDefault();



            update.City = user.City;

            update.Email = user.Email;

            update.FatherName = user.FatherName;
            update.FirstName = user.FirstName;
            update.LastName = user.LastName;
            update.PhoneNumber = user.PhoneNumber;
            update.Flat = user.Flat;
            update.House = user.House;


            update.NormalizedEmail = user.Email.Normalize();


            update.Street = user.Street;

            Data.SaveChanges();

            return RedirectToAction("Profile");


        }
        [Authorize]
        public IActionResult AdminProfile()
        {
            var result = new ProfileViewModel
            {
                Histories = Data.Histories,
                Promotion = Data.Promotions,
                AppUser = Data.Users,
                ProductQuantity = Data.ProductsQuantity,
                Cart = cart,
                FeedBacks = Data.FeedBacks

            };
            return View(result);
        }

        [Authorize]
        public IActionResult OpenFeedBack()
        {
            return View(Data.FeedBacks.OrderBy(q=>q.id).Where(q=>q.TypeFeedback==false));
        }
        [Authorize]
        public IActionResult ClosedFeedBack()
        {

            return View(Data.FeedBacks.OrderBy(q => q.id).Where(q => q.TypeFeedback == false));
        }


        [Authorize]
        public IActionResult EditFeedBack(int id) => View(Data.FeedBacks.FirstOrDefault(q => q.id == id));

        [Authorize]
        [HttpPost]
        public IActionResult EditFeedBack(FeedBack feedBack, int id, bool Status , string Comment)
        {
            if (Status)
            {
                FeedBack update = Data.FeedBacks.FirstOrDefault(q => q.id == id);

                if (update.AdminCommentaries==null)
                {
                    update.AdminCommentaries = $"{DateTime.Now}\n  {Comment}";
                }
                else
                {
                    update.AdminCommentaries = $"{update.AdminCommentaries}  \n {DateTime.Now} \n  {Comment}";
                }             
                update.FeedbakStatus = true;
                update.Admin = User.Identity.Name;
                update.Closed = DateTime.Now;

                Data.SaveChanges();
                return RedirectToAction("OpenFeedBack");
            }
            else
            {
                FeedBack update = Data.FeedBacks.FirstOrDefault(q => q.id == id);

                if (update.AdminCommentaries == null)
                {
                    update.AdminCommentaries = $"{DateTime.Now}\n  {Comment}";
                }
                else
                {
                    update.AdminCommentaries = $"{update.AdminCommentaries}  \n {DateTime.Now}\n  {Comment}";
                }
                update.FeedbakStatus = false;
                update.Admin = User.Identity.Name;
                update.Closed = DateTime.Now;

                Data.SaveChanges();
                return RedirectToAction("ClosedFeedBack");
            }
        }



        [Authorize]
        public IActionResult OpenGaranteeRequest()
        {
            return View(Data.FeedBacks.OrderBy(q => q.id).Where(q => q.TypeFeedback == true));
        }

        [Authorize]
        public IActionResult ClosedGaranteeRequest()
        {

            return View(Data.FeedBacks.OrderBy(q => q.id).Where(q => q.TypeFeedback == true));
        }


        [Authorize]
        public IActionResult EditGaranteeRequest(int id) => View(Data.FeedBacks.FirstOrDefault(q => q.id == id));


        [Authorize]
        [HttpPost]
        public IActionResult EditGaranteeRequest(FeedBack feedBack, int id, bool Status, string Comment)
        {
            if (Status)
            {
                FeedBack update = Data.FeedBacks.FirstOrDefault(q => q.id == id);

                if (update.AdminCommentaries == null)
                {
                    update.AdminCommentaries = $"{DateTime.Now}\n  {Comment}";
                }
                else
                {
                    update.AdminCommentaries = $"{update.AdminCommentaries}  \n {DateTime.Now} \n  {Comment}";
                }
                update.FeedbakStatus = true;
                update.Admin = User.Identity.Name;
                update.Closed = DateTime.Now;

                Data.SaveChanges();
                return RedirectToAction("OpenGaranteeRequest");
            }
            else
            {
                FeedBack update = Data.FeedBacks.FirstOrDefault(q => q.id == id);

                if (update.AdminCommentaries == null)
                {
                    update.AdminCommentaries = $"{DateTime.Now}\n  {Comment}";
                }
                else
                {
                    update.AdminCommentaries = $"{update.AdminCommentaries}  \n {DateTime.Now}\n  {Comment}";
                }
                update.FeedbakStatus = false;
                update.Admin = User.Identity.Name;
                update.Closed = DateTime.Now;

                Data.SaveChanges();
                return RedirectToAction("ClosedGaranteeRequest");
            }
        }
    }
}
