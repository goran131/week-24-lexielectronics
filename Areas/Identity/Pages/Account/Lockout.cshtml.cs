// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LexiElectronics.Areas.Identity.Pages.Account;

/// <summary>
///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
///     directly from your code. This API may change or be removed in future releases.
/// </summary>
[AllowAnonymous]
public class LockoutModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public bool IsPermanentlyLockedOut { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public void OnGet(bool isPermanentlyLockedOut)
    {
        IsPermanentlyLockedOut = isPermanentlyLockedOut;
    }
}
