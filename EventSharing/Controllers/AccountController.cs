using AutoMapper;
using EventSharing.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EventSharing.Controllers
{
    public class AccountController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [HttpGet]
        public async Task<IActionResult> ChangePasswordAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return BadRequest("L'utilisateur n'existe pas");
            }

            var isTokenValid = await _userManager.VerifyUserTokenAsync(
                user, _userManager.Options.Tokens.PasswordResetTokenProvider,
                TokenOptions.DefaultEmailProvider, token);

            if (!isTokenValid)
            {
                return BadRequest("Token invalide");
            }

            var changePasswordVm = new ChangePasswordViewModel
            {
                Email = email,
            };
            return View(changePasswordVm);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string? id, [Bind("Password,ConfirmPassword")] ChangePasswordViewModel changePasswordVm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, changePasswordVm.Password);
                if (result.Succeeded)
                {
                    //return Json(new { success = true });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
            return Json(new { success = false, errors = errors });
        }

        [HttpGet]
        public IActionResult ChangePasswordConfirmed()
        {
            return View();
        }
    }
}
