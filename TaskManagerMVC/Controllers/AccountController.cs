using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerMVC.Services;

namespace TaskManagerMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Account/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            // If the user is already authenticated, redirect to the home page
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Task");
            }

            return View();
        }



        // POST: Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            var token = await _apiService.LoginAsync(email, password);

            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Error = "Invalid login attempt. Please try again.";
                return View();
            }

            // Authenticate the user
            var claims = new[] { new Claim(ClaimTypes.Name, email) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Store the token
            HttpContext.Session.SetString("AuthToken", token);

            // Redirect to the original URL or to the home page
            return Redirect(returnUrl ?? Url.Action("Index", "Task"));
        }


        public async Task<IActionResult> Logout()
        {
            // Clear the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
