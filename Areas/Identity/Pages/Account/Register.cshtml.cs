// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using LexiElectronics.Data;
using LexiElectronics.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;

namespace LexiElectronics.Areas.Identity.Pages.Account;

public class RegisterModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserStore<ApplicationUser> _userStore;
    private readonly IUserEmailStore<ApplicationUser> _emailStore;
    private readonly ILogger<RegisterModel> _logger;
    private readonly IEmailSender _emailSender;
    private readonly ApplicationDbContext _appDbContext;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterModel(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore, 
                         SignInManager<ApplicationUser> signInManager, ILogger<RegisterModel> logger, 
                         IEmailSender emailSender, ApplicationDbContext appDbContext, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
        _appDbContext = appDbContext;
        _roleManager = roleManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    public List<SelectListItem> UserRoleListItems { get; set; } = new List<SelectListItem>();


    public class InputModel
    {
        [Display(Name = "Användarroll")]

        public string? UserRoleId { get; set; }

        [Required]
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; } = default!;

        [Required]
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; } = default!;

        [Required]
        [EmailAddress]
        [Display(Name = "Epost")]
        public string Email { get; set; } = default!;

        [Display(Name = "mobil")]
        public string? PhoneNumber { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; } = default!;


        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        [Compare("Password", ErrorMessage = "Lösenord och bekräftat lösenord stämmer inte.")]
        public string? ConfirmPassword { get; set; }


        [Required]
        [Display(Name = "Namn")]
        public string InvoiceName { get; set; } = default!;

        [Required]
        [Display(Name = "Gatuadress")]
        public string InvoiceStreetAddress { get; set; } = default!;

        [Required]
        [Display(Name = "Postnummer")]
        public string InvoiceZipcode { get; set; } = default!;

        [Required]
        [Display(Name = "Ort")]
        public string InvoiceCity { get; set; } = default!;


        [Display(Name = "Namn")]
        public string? DeliveryName { get; set; }
        [Display(Name = "Gatuadress")]
        public string? DeliveryStreetAddress { get; set; }

        [Display(Name = "Postnummer")]
        public string? DeliveryZipcode { get; set; }

        [Display(Name = "Ort")]
        public string? DeliveryCity { get; set; }
    }



    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        // Populate role select list for the view
        UserRoleListItems = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        // Ensure the role list is available if we need to redisplay the form
       
        UserRoleListItems = _roleManager.Roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name }).ToList();

        if (ModelState.IsValid)
        {
            var user = CreateUser();

            user.PhoneNumber = Input.PhoneNumber;
            user.Firstname = Input.Firstname;
            user.Lastname = Input.Lastname;
            user.InvoiceName = Input.InvoiceName;
            user.InvoiceStreetAddress = Input.InvoiceStreetAddress;
            user.InvoiceZipcode= Input.InvoiceZipcode;
            user.InvoiceCity = Input.InvoiceCity;
            user.DeliveryName = Input.DeliveryName;
            user.DeliveryStreetAddress = Input.DeliveryStreetAddress;
            user.DeliveryZipcode = Input.DeliveryZipcode;
            user.DeliveryCity = Input.DeliveryCity;
            user.PreventAccess = false;
            string userRoleId = Input.UserRoleId;

            if (string.IsNullOrEmpty(userRoleId)) {
                userRoleId = 3.ToString(); // Default to "Customer" role if no role is selected
            }

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            
            var result = await _userManager.CreateAsync(user, Input.Password);

            IdentityRole userRole = await _roleManager.FindByIdAsync(userRoleId);               
            await _userManager.AddToRoleAsync(user, userRole.Name!);


            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme)!;

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private ApplicationUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<ApplicationUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<ApplicationUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
        {
            throw new NotSupportedException("The default UI requires a user store with email support.");
        }
        return (IUserEmailStore<ApplicationUser>)_userStore;
    }


}
