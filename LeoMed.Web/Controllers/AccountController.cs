﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LeoMed.Model;
using LeoMed.Web.Models.AccountViewModels;
using LeoMed.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LeoMed.Web.Controllers
{
     [Authorize]
     public class AccountController : Controller
     {

          private readonly RoleManager<IdentityRole> _roleManager;
          private readonly UserManager<AppUser> _userManager;
          private readonly SignInManager<AppUser> _signInManager;
          private readonly IEmailSender _emailSender;
          private readonly ISmsSender _smsSender;
          private readonly ILogger _logger;
          private readonly string _externalCookieScheme;

          AppDbContext db = new AppDbContext();

          public AccountController(
              UserManager<AppUser> userManager,
              SignInManager<AppUser> signInManager,
              IOptions<IdentityCookieOptions> identityCookieOptions,
              RoleManager<IdentityRole> roleManager,
              IEmailSender emailSender,
              ISmsSender smsSender,
              ILoggerFactory loggerFactory)
          {
               _userManager = userManager;
               _roleManager = roleManager;
               _signInManager = signInManager;
               _externalCookieScheme = identityCookieOptions.Value.ExternalCookieAuthenticationScheme;
               _emailSender = emailSender;
               _smsSender = smsSender;
               _logger = loggerFactory.CreateLogger<AccountController>();
          }

          //
          // GET: /Account/Login
          [HttpGet]
          [AllowAnonymous]
          public async Task<IActionResult> Login(string returnUrl = null)
          {
               // Clear the existing external cookie to ensure a clean login process
               await HttpContext.Authentication.SignOutAsync(_externalCookieScheme);

               ViewData["ReturnUrl"] = returnUrl;
               return View();
          }

          //
          // POST: /Account/Login
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
          {
               ViewData["ReturnUrl"] = returnUrl;
               if (ModelState.IsValid)
               {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                         _logger.LogInformation(1, "User logged in.");
                         return RedirectToLocal(returnUrl);
                    }
                    if (result.RequiresTwoFactor)
                    {
                         return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    }
                    if (result.IsLockedOut)
                    {
                         _logger.LogWarning(2, "User account locked out.");
                         return View("Lockout");
                    }
                    else
                    {
                         ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                         return View(model);
                    }
               }

               // If we got this far, something failed, redisplay form
               return View(model);
          }

          //
          // GET: /Account/Register
          [HttpGet]
          [AllowAnonymous]
          public IActionResult Register(string returnUrl = null)
          {
               ViewData["ReturnUrl"] = returnUrl;
               return View();
          }

          //
          // POST: /Account/Register
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
          {
               ViewData["ReturnUrl"] = returnUrl;
               if (ModelState.IsValid)
               {
                    var user = new AppUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                         await _signInManager.SignInAsync(user, isPersistent: false);
                         _logger.LogInformation(3, "User created a new account with password.");
                         return RedirectToAction("Success");
                    }
                    AddErrors(result);
               }

               // If we got this far, something failed, redisplay form
               return View(model);
          }

          //
          // POST: /Account/Logout
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Logout()
          {
               await _signInManager.SignOutAsync();
               _logger.LogInformation(4, "User logged out.");
               return RedirectToAction(nameof(HomeController.Index), "Home");
          }

          //
          // POST: /Account/ExternalLogin
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public IActionResult ExternalLogin(string provider, string returnUrl = null)
          {
               // Request a redirect to the external login provider.
               var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
               var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
               return Challenge(properties, provider);
          }

          //
          // GET: /Account/ExternalLoginCallback
          [HttpGet]
          [AllowAnonymous]
          public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
          {
               if (remoteError != null)
               {
                    ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                    return View(nameof(Login));
               }
               var info = await _signInManager.GetExternalLoginInfoAsync();
               if (info == null)
               {
                    return RedirectToAction(nameof(Login));
               }

               // Sign in the user with this external login provider if the user already has a login.
               var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
               if (result.Succeeded)
               {
                    _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                    return RedirectToLocal(returnUrl);
               }
               if (result.RequiresTwoFactor)
               {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
               }
               if (result.IsLockedOut)
               {
                    return View("Lockout");
               }
               else
               {
                    // If the user does not have an account, then ask the user to create an account.
                    ViewData["ReturnUrl"] = returnUrl;
                    ViewData["LoginProvider"] = info.LoginProvider;
                    var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
               }
          }

          //
          // POST: /Account/ExternalLoginConfirmation
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
          {
               if (ModelState.IsValid)
               {
                    // Get the information about the user from the external login provider
                    var info = await _signInManager.GetExternalLoginInfoAsync();
                    if (info == null)
                    {
                         return View("ExternalLoginFailure");
                    }
                    var user = new AppUser { UserName = model.Email, Email = model.Email };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                         result = await _userManager.AddLoginAsync(user, info);
                         if (result.Succeeded)
                         {
                              await _signInManager.SignInAsync(user, isPersistent: false);
                              _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                              return RedirectToLocal(returnUrl);
                         }
                    }
                    AddErrors(result);
               }

               ViewData["ReturnUrl"] = returnUrl;
               return View(model);
          }

          // GET: /Account/ConfirmEmail
          [HttpGet]
          [AllowAnonymous]
          public async Task<IActionResult> ConfirmEmail(string userId, string code)
          {
               if (userId == null || code == null)
               {
                    return View("Error");
               }
               var user = await _userManager.FindByIdAsync(userId);
               if (user == null)
               {
                    return View("Error");
               }
               var result = await _userManager.ConfirmEmailAsync(user, code);
               return View(result.Succeeded ? "ConfirmEmail" : "Error");
          }

          //
          // GET: /Account/ForgotPassword
          [HttpGet]
          [AllowAnonymous]
          public IActionResult ForgotPassword()
          {
               return View();
          }

          //
          // POST: /Account/ForgotPassword
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
          {
               if (ModelState.IsValid)
               {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                         // Don't reveal that the user does not exist or is not confirmed
                         return View("ForgotPasswordConfirmation");
                    }
               }

               // If we got this far, something failed, redisplay form
               return View(model);
          }

          //
          // GET: /Account/ForgotPasswordConfirmation
          [HttpGet]
          [AllowAnonymous]
          public IActionResult ForgotPasswordConfirmation()
          {
               return View();
          }

          //
          // GET: /Account/ResetPassword
          [HttpGet]
          [AllowAnonymous]
          public IActionResult ResetPassword(string code = null)
          {
               return code == null ? View("Error") : View();
          }

          //
          // POST: /Account/ResetPassword
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
          {
               if (!ModelState.IsValid)
               {
                    return View(model);
               }
               var user = await _userManager.FindByEmailAsync(model.Email);
               if (user == null)
               {
                    // Don't reveal that the user does not exist
                    return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
               }
               var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
               if (result.Succeeded)
               {
                    return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
               }
               AddErrors(result);
               return View();
          }

          //
          // GET: /Account/ResetPasswordConfirmation
          [HttpGet]
          [AllowAnonymous]
          public IActionResult ResetPasswordConfirmation()
          {
               return View();
          }

          //
          // GET: /Account/SendCode
          [HttpGet]
          [AllowAnonymous]
          public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
          {
               var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
               if (user == null)
               {
                    return View("Error");
               }
               var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
               var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
               return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
          }

          //
          // POST: /Account/SendCode
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> SendCode(SendCodeViewModel model)
          {
               if (!ModelState.IsValid)
               {
                    return View();
               }

               var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
               if (user == null)
               {
                    return View("Error");
               }

               // Generate the token and send it
               var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
               if (string.IsNullOrWhiteSpace(code))
               {
                    return View("Error");
               }

               var message = "Your security code is: " + code;
               if (model.SelectedProvider == "Email")
               {
                    await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
               }
               else if (model.SelectedProvider == "Phone")
               {
                    await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
               }

               return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
          }

          //
          // GET: /Account/VerifyCode
          [HttpGet]
          [AllowAnonymous]
          public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
          {
               // Require that the user has already logged in via username/password or external login
               var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
               if (user == null)
               {
                    return View("Error");
               }
               return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
          }

          //
          // POST: /Account/VerifyCode
          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
          {
               if (!ModelState.IsValid)
               {
                    return View(model);
               }

               // The following code protects for brute force attacks against the two factor codes.
               // If a user enters incorrect codes for a specified amount of time then the user account
               // will be locked out for a specified amount of time.
               var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
               if (result.Succeeded)
               {
                    return RedirectToLocal(model.ReturnUrl);
               }
               if (result.IsLockedOut)
               {
                    _logger.LogWarning(7, "User account locked out.");
                    return View("Lockout");
               }
               else
               {
                    ModelState.AddModelError(string.Empty, "Invalid code.");
                    return View(model);
               }
          }

          //
          // GET /Account/CreatePro
          [HttpGet]
          public IActionResult CreatePro()
          {
               return View();
          }

          //
          // GET /Account/CreatePro
          [HttpPost]
          public async Task<IActionResult> CreatePro(IFormCollection formCollection)
          {
               Professional prof = new Professional();

               var user = await db.AppUsers.SingleAsync(e => e.UserName == User.Identity.Name);

               prof.Description = formCollection["Description"];
               prof.Experience = Int32.Parse(formCollection["Experience"]);
               prof.Profession = formCollection["Profession"];

               user.Title = formCollection["Title"];
               user.Firstname = formCollection["Firstname"];
               user.Middlename = formCollection["Middlename"];
               user.Lastname = formCollection["Lastname"];
               user.Sex = formCollection["Sex"];
               user.DateOfBirth = DateTime.Parse(formCollection["DateOfBirth"]);

               prof.AppUser = user;
               IdentityRole role = new IdentityRole("professional");
               //bool roleResult = await _roleManager.RoleExistsAsync(role.Name);
               

               if (ModelState.IsValid)
               {
                    await db.Professionals.AddAsync(prof);
                    db.AppUsers.Attach(user);
                    db.Entry(prof).State = EntityState.Added;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //if (roleResult)
                    //{
                    //     await _userManager.AddToRoleAsync(user, role.Name);
                    //}
                    //else
                    //{
                    //     await _roleManager.CreateAsync(role);
                    //     await _userManager.AddToRoleAsync(user, role.Name);
                    //}

                    return Redirect("/Professionals");
               }

               return View();
          }


          //
          // GET /Account/CreatePatient
          [HttpGet]
          public IActionResult CreatePatient()
          {
               return View();
          }

          //
          // GET /Account/CreatePro
          [HttpPost]
          public async Task<IActionResult> CreatePatient(IFormCollection formCollection)
          {
               Patient patient = new Patient();

               var user = db.AppUsers.Single(e => e.UserName == User.Identity.Name);

               patient.PatientNo = Guid.NewGuid().ToString().Substring(0, 10).Replace(" ", "-").ToUpper();
               
               patient.Location = formCollection["Location"];
               patient.State = formCollection["State"];
               patient.Country = formCollection["Country"];

               user.Title = formCollection["Title"];
               user.Firstname = formCollection["Firstname"];
               user.Middlename = formCollection["Middlename"];
               user.Lastname = formCollection["Lastname"];
               user.Sex = formCollection["Sex"];
               user.DateOfBirth = DateTime.Parse(formCollection["DateOfBirth"]);

               patient.AppUser = user;

               IdentityRole role = new IdentityRole("patient");
               //bool roleResult = await _roleManager.RoleExistsAsync(role.Name);


               if (ModelState.IsValid)
               {
                    await db.Patients.AddAsync(patient);
                    db.AppUsers.Attach(user);
                    db.Entry(patient).State = EntityState.Added;
                    db.Entry(user).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    //if (roleResult)
                    //{
                    //     await _userManager.AddToRoleAsync(user, role.Name);
                    //}
                    //else
                    //{
                    //     await _roleManager.CreateAsync(role);
                    //     await _userManager.AddToRoleAsync(user, role.Name);
                    //}

                    return Redirect("/Patients");
               }

               return View();
          }

          //
          // GET /Account/Success
          [HttpGet]
          public IActionResult Success()
          {
               return View();
          }

          //
          // GET /Account/AccessDenied
          [HttpGet]
          public IActionResult AccessDenied()
          {
               return View();
          }

          #region Helpers

          private void AddErrors(IdentityResult result)
          {
               foreach (var error in result.Errors)
               {
                    ModelState.AddModelError(string.Empty, error.Description);
               }
          }

          private IActionResult RedirectToLocal(string returnUrl)
          {
               if (Url.IsLocalUrl(returnUrl))
               {
                    return Redirect(returnUrl);
               }
               else
               {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
               }
          }

          #endregion
     }
}
