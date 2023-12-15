using IndigoProject.Helper;
using IndigoProject.Models;
using IndigoProject.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IndigoProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly RoleManager<IdentityRole> _roleManager;




        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signinManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
        }


        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AppUserVM appUserVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser _appuser = new AppUser
            {
                Name = appUserVM.Name,
                Email = appUserVM.Email,
                Surname = appUserVM.Surname,
                UserName = appUserVM.Username


            };

            var create = await _userManager.CreateAsync(_appuser, appUserVM.Password);
            if (!create.Succeeded)
            {
                foreach (var item in create.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(appUserVM);
            }

            await _userManager.AddToRoleAsync(_appuser, UserRole.Member.ToString());

            return RedirectToAction(nameof(Login), "Account");

        }

           public IActionResult Login(string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm, string? returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("~/");
            /*           if (Url.IsLocalUrl(returnUrl))
                       {
                           return View(returnUrl);
                       };*/

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVm.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is invalid");
                return View(loginVm);
            }
            SignInResult result = await _signinManager.PasswordSignInAsync(user, loginVm.Password, loginVm.RememberMe, true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Your Account Is Lock Out");
                return View(loginVm);
            }

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Ya Email ya da Password sehvdir");
                return View(loginVm);
            }
            if (returnUrl == null)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            return Redirect(returnUrl);


        }

        public async Task<IActionResult> Logout()
        {
            _signinManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");

        }

        public async Task<IActionResult> CreateRole()
        {
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if (await _roleManager.FindByNameAsync(item.ToString()) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString(),
                    });
                }
            }

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}
