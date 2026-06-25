// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LexiElectronics.Models;

namespace LexiElectronics.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? Username { get; set; }

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

    private async Task LoadAsync(ApplicationUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;

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
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
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

        var setUserResult = await _userManager.UpdateAsync(user);

        if (!setUserResult.Succeeded)
        {
            StatusMessage = "Unexpected error when trying to set phone number.";
            return RedirectToPage();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}
