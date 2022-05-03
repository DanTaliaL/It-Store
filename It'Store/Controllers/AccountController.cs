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

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, DataContext Data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.Data = Data;
        }

        [AllowAnonymous]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return RedirectToAction(ReturnUrl??"Index","Home");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string? ReturnUrl)
        {
            if (model.Login == null || model.Password == null)
            {
                ModelState.AddModelError(nameof(CreateUserModel.Password),"");
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
        public async Task<IActionResult> UserCreate(CreateUserModel model)
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
            var result = new HistoryViewModel
            {
                CartLine = Data.CartLine.Include(q=>q.Product),
                Order =Data.Orders.Include(q=>q.Lines).Where(q=>q.Name==User.Identity.Name)
            };
            return View(result);
        }
        public IActionResult AdminProfile()
        {
            var result = new HistoryViewModel
            {
                CartLine = Data.CartLine.Include(q => q.Product),
                Order = Data.Orders.Include(q => q.Lines).Where(q => q.Name == User.Identity.Name)
            };
            return View(result);
        }
    }
}
