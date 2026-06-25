// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using LexiElectronics.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LexiElectronics.Areas.Identity.Pages.Account.Manage;

public class EditUserModel : PageModel
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;

    public EditUserModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? UserName { get; set; }

    public string? UserId { get; set; }

    public List<SelectListItem> UserListItems { get; set; } = new List<SelectListItem>();

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string? StatusMessage { get; set; }

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
    public class InputModel
    {
        [Required]
        [Display(Name = "Användarnamn")]
        public string UserName { get; set; } = default!;


        [Required]
        [Display(Name = "Epost")]
        public string Email { get; set; } = default!;

        [Required]
        [Display(Name = "Förnamn")]
        public string Firstname { get; set; } = default!;

        [Required]
        [Display(Name = "Efternamn")]
        public string Lastname { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string? PhoneNumber { get; set; }

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

    public async Task<IActionResult> OnPostSelectUserAsync(string UserId)
    {
        var user = await userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{UserId}'.");
        }
        await LoadAsync(user);
        return Page();
    }
    private async Task LoadAsync(ApplicationUser user)
    {
        var userName = await userManager.GetUserNameAsync(user);
        var phoneNumber = await userManager.GetPhoneNumberAsync(user);

        UserName = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            Firstname = user.Firstname,
            Lastname = user.Lastname,
            InvoiceName = user.InvoiceName,
            InvoiceStreetAddress = user.InvoiceStreetAddress,
            InvoiceZipcode = user.InvoiceZipcode,
            InvoiceCity = user.InvoiceCity,
            DeliveryName = user.DeliveryName,
            DeliveryStreetAddress = user.DeliveryStreetAddress,
            DeliveryZipcode = user.DeliveryZipcode,
            DeliveryCity = user.DeliveryCity
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        UserListItems = userManager.Users.Select(u => new SelectListItem { Value = u.Id, Text = (u.Firstname + " " + u.Lastname) }).ToList();

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
        }

        UserListItems = userManager.Users.Select(u => new SelectListItem { Value = u.Id, Text = (u.Firstname + " " + u.Lastname) }).ToList();
        
        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        user.Firstname = Input.Firstname;
        user.Lastname = Input.Lastname;
        user.InvoiceName = Input.InvoiceName;
        user.InvoiceStreetAddress = Input.InvoiceStreetAddress;
        user.InvoiceZipcode = Input.InvoiceZipcode;
        user.InvoiceCity = Input.InvoiceCity;
        user.DeliveryName = Input.DeliveryName;
        user.DeliveryStreetAddress = Input.DeliveryStreetAddress;
        user.DeliveryZipcode = Input.DeliveryZipcode;
        user.DeliveryCity = Input.DeliveryCity;

        var setUserResult = await userManager.UpdateAsync(user);

        if (!setUserResult.Succeeded)
        {
            StatusMessage = "Unexpected error when trying to set phone number.";
            return RedirectToPage();
        }

        await signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}
