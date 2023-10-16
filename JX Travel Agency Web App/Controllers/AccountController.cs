using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Data.Enums;
using JX_Travel_Agency_Web_App.Models;
using JX_Travel_Agency_Web_App.Models.QueryModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace JX_Travel_Agency_Web_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService authenticationService;
        private readonly AppDbContext db;

        public AccountController(IAuthenticationService authenticationService, AppDbContext db)
        {
            this.authenticationService = authenticationService;
            this.db = db;

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserQueryModel user)
        {
            if (ModelState.IsValid)
            {
                if (IsUserValid(user.Username,user.Password))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        IsAdmin(user.Username) ? new Claim(ClaimTypes.Role, UserRoles.Admin.ToString()):new Claim(ClaimTypes.Role, UserRoles.User.ToString())
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,principal);
                    
                    return RedirectToAction("Index", "Home");
                } else
                {
					TempData["ErrorMsg"] = "Username/Password Incorrect!";
				}
            }
            
            return View(user);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
		public IActionResult Register(User user)
        {
            try
            {
				if (ModelState.IsValid)
				{
					db.Users.Add(user);
					db.SaveChanges();
					return View("Login");
				}
			}
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                TempData["msg"]="Username already exists!";
				return View(user);
			}
            return View(user);
		}

		public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        public bool IsUserValid(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user!=null)
            {
				if (user.Password.Equals(password))
				{
					return true;
				}
			}
            
            return false;
        }

        public bool IsAdmin(string username)
        {
            var user = db.Users.FirstOrDefault(u => u.Username == username);
            if (user.Role==UserRoles.Admin)
            {
                return true;
            }
            return false;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
