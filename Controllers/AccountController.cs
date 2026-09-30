using CMS_HotelBooking.Models;
using CMS_HotelBooking.Services.Interfaces;
using CMS_HotelBooking.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS_HotelBooking.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        public AccountController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // Open register page
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // Submit register form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new Users
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim().ToLowerInvariant(),
                CountryCode = model.CountryCode.Trim(),
                Phone = model.Phone.Trim()
            };

            var (success, message) =
                await _usersService.RegisterAsync(
                    user,
                    model.Password,
                    "Customer"
                );

            if (!success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    message
                );

                return View(model);
            }

            TempData["SuccessMessage"] =
                "Registration successful. Please login to continue.";

            return RedirectToAction("Login");
        }

        // Open login page
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true) return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // Submit login form
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var (success, user, message) = await _usersService.LoginAsync(model.Email, model.Password);

            if (!success || user == null)
            {
                ModelState.AddModelError(string.Empty, message);
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            if (!string.Equals(user.Role, model.LoginAs, StringComparison.OrdinalIgnoreCase))
            {
                string expected = user.Role == "Admin" ? "Admin" : "User";
                ModelState.AddModelError(string.Empty, 
                    $"This account is registered as {expected}. Please select \"{expected}\" from the Login As dropdown.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = model.RememberMe });

            if (user.Role == "Admin")
                return RedirectToAction("Dashboard", "Dashboard", new { area = "Admin" });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        // Logout user
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        // Open access denied page
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Open forgot password page
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // Submit forgot password form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(
                    "email",
                    "Please enter your email address."
                );

                return View();
            }

            var result = await _usersService.SendResetCodeAsync(email);

            if (!result.Success)
            {
                ModelState.AddModelError(
                    string.Empty,
                    result.Message
                );

                {
                    return View();
                }
            }

            TempData["SuccessMessage"] = result.Message;

            return RedirectToAction(
                nameof(VerifyResetCode),
                new { email = email }
            );
        }

        // Open verify reset code page
        [HttpGet]
        public IActionResult VerifyResetCode(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return RedirectToAction(nameof(ForgotPassword));
            }

            ViewBag.Email = email;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyResetCode
            (
                string email, 
                string code, 
                string newPassword, 
                string confirmPassword
            )
        {
            ViewBag.Email = email;

            if (string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError(string.Empty, "Please enter the verification code sent to your email.");
                {
                    return View();
                }
            }

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
            {
                ModelState.AddModelError(string.Empty, "Password must be at least 6 characters long.");
                {
                    return View();
                }
            }

            if (newPassword != confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                {
                    return View();
                }
            }

            var result = await _usersService.ResetPasswordAsync(email, code, newPassword);

            if (!result.Success)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }

            TempData["SuccessMessage"] = result.Message;
            {
                return RedirectToAction(nameof(Login));
            }
        }
    }
}
