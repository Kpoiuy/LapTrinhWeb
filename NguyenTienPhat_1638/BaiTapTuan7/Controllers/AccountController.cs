using BaiTapTuan7.Models;
using BaiTapTuan7.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaiTapTuan7.Controllers;

[Route("account")]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<AccountController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpGet("register")]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Register validation failed.");
            return View(model);
        }

        var user = new IdentityUser
        {
            UserName = model.Username.Trim(),
            Email = model.Email.Trim(),
            EmailConfirmed = true
        };

        var createResult = await _userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            _logger.LogWarning("Register failed for username {Username}.", user.UserName);
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        if (!await _roleManager.RoleExistsAsync(RoleNames.Student))
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleNames.Student));
        }

        await _userManager.AddToRoleAsync(user, RoleNames.Student);
        await _signInManager.SignInAsync(user, isPersistent: false);
        _logger.LogInformation("User {UserId} registered and assigned role STUDENT.", user.Id);

        TempData["Success"] = "Đăng ký thành công. Vai trò mặc định của bạn là STUDENT.";
        return Redirect("/home");
    }

    [AllowAnonymous]
    [HttpGet("login")]
    public async Task<IActionResult> Login(string? returnUrl = null)
    {
        var vm = new LoginViewModel();
        await PopulateLoginMetadataAsync(vm, returnUrl);
        return View(vm);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        await PopulateLoginMetadataAsync(model, returnUrl);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            model.Username.Trim(),
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            _logger.LogInformation("User {Username} logged in successfully.", model.Username);
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("/home");
        }

        _logger.LogWarning("Login failed for username {Username}.", model.Username);
        ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
        return View(model);
    }

    [Authorize]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var userId = _userManager.GetUserId(User);
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User {UserId} logged out.", userId);
        return Redirect("/home");
    }

    [AllowAnonymous]
    [HttpPost("external-login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl = null)
    {
        if (!await IsGoogleProviderEnabledAsync())
        {
            _logger.LogWarning("External login requested but Google provider is not configured.");
            TempData["Error"] = "Google OAuth chưa được cấu hình. Vui lòng điền ClientId và ClientSecret trong appsettings.";
            return RedirectToAction(nameof(Login));
        }

        if (!string.Equals(provider, "Google", StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToAction(nameof(Login));
        }

        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [AllowAnonymous]
    [HttpGet("external-login-callback")]
    public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
    {
        if (!string.IsNullOrWhiteSpace(remoteError))
        {
            _logger.LogWarning("Google login returned remote error: {RemoteError}", remoteError);
            TempData["Error"] = $"Lỗi đăng nhập Google: {remoteError}";
            return RedirectToAction(nameof(Login));
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info is null)
        {
            _logger.LogWarning("External login callback without login info.");
            TempData["Error"] = "Không thể tải thông tin đăng nhập ngoài.";
            return RedirectToAction(nameof(Login));
        }

        var signInResult = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (signInResult.Succeeded)
        {
            _logger.LogInformation("External login succeeded for provider {Provider}.", info.LoginProvider);
            return RedirectToLocal(returnUrl);
        }

        var email = info.Principal.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrWhiteSpace(email))
        {
            email = $"{info.ProviderKey}@google.local";
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            var baseName = email.Split('@')[0];
            var userName = await GenerateUniqueUserNameAsync(baseName);
            user = new IdentityUser
            {
                UserName = userName,
                Email = email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user);
            if (!createResult.Succeeded)
            {
                _logger.LogWarning("Failed to create local user for Google login email {Email}.", email);
                TempData["Error"] = "Không thể tạo tài khoản cục bộ từ Google.";
                return RedirectToAction(nameof(Login));
            }

            _logger.LogInformation("Created local account {UserId} from Google login.", user.Id);
        }

        var addLoginResult = await _userManager.AddLoginAsync(user, info);
        if (!addLoginResult.Succeeded)
        {
            var duplicated = addLoginResult.Errors.Any(e => e.Code.Contains("LoginAlreadyAssociated", StringComparison.OrdinalIgnoreCase));
            if (!duplicated)
            {
                _logger.LogWarning("Failed to link Google account for user {UserId}.", user.Id);
                TempData["Error"] = "Không thể liên kết tài khoản Google.";
                return RedirectToAction(nameof(Login));
            }
        }

        if (!await _roleManager.RoleExistsAsync(RoleNames.Student))
        {
            await _roleManager.CreateAsync(new IdentityRole(RoleNames.Student));
        }

        if (!await _userManager.IsInRoleAsync(user, RoleNames.Student))
        {
            await _userManager.AddToRoleAsync(user, RoleNames.Student);
        }

        await _signInManager.SignInAsync(user, isPersistent: false);
        _logger.LogInformation("Google login completed for user {UserId}.", user.Id);
        return RedirectToLocal(returnUrl);
    }

    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private async Task PopulateLoginMetadataAsync(LoginViewModel model, string? returnUrl)
    {
        model.GoogleLoginEnabled = await IsGoogleProviderEnabledAsync();
        model.ReturnUrl = returnUrl;
    }

    private async Task<bool> IsGoogleProviderEnabledAsync()
    {
        var externalSchemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
        return externalSchemes.Any(s => string.Equals(s.Name, "Google", StringComparison.OrdinalIgnoreCase));
    }

    private async Task<string> GenerateUniqueUserNameAsync(string baseUserName)
    {
        var sanitized = string.IsNullOrWhiteSpace(baseUserName) ? "student" : baseUserName.Trim();
        var candidate = sanitized;
        var index = 1;

        while (await _userManager.FindByNameAsync(candidate) is not null)
        {
            candidate = $"{sanitized}{index}";
            index++;
        }

        return candidate;
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return Redirect("/home");
    }
}
