using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventSharing.Data;
using EventSharing.ViewModels;
using EventSharing.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace EventSharing.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public UsersController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return _context.Set<User>() != null ? 
                          View(_mapper.Map<List<UserViewModel>>(await _context.Set<User>().ToListAsync())) :
                          Problem("Entity set 'ApplicationDbContext.UserViewModel'  is null.");
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Set<User>() == null)
            {
                return NotFound();
            }

            var userViewModel = _mapper.Map<UserViewModel>(await _context.Set<User>()
                .FirstOrDefaultAsync(m => m.Id == id));
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Password,ConfirmPassword,PhoneNumber,EmailConfirmed")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(userViewModel);
                user.Id = Guid.NewGuid().ToString();
                user.UserName = userViewModel.Email;
                var result = await _userManager.CreateAsync(user, userViewModel.Password);
                if (result.Succeeded)
                {
                    // envoi email
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        userViewModel.Email,
                        "Reset Password",
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(userViewModel);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Set<User>() == null)
            {
                return NotFound();
            }

            var userViewModel = _mapper.Map<UserViewModel>(await _context.Set<User>()
                .FirstOrDefaultAsync(m => m.Id == id));
            if (userViewModel == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Email,PhoneNumber,EmailConfirmed")] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _context.Set<User>().FindAsync(id);
                    user.Name = userViewModel.Name;
                    user.PhoneNumber = userViewModel.PhoneNumber;
                    user.EmailConfirmed = userViewModel.EmailConfirmed;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserViewModelExists(userViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Set<User>() == null)
            {
                return NotFound();
            }

            var userViewModel = _mapper.Map<UserViewModel>(await _context.Set<User>()
                .FirstOrDefaultAsync(m => m.Id == id));
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Set<User>() == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserViewModel'  is null.");
            }
            var user = await _context.Set<User>().FindAsync(id);
            if (user != null)
            {
                _context.Set<User>().Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserViewModelExists(string id)
        {
          return (_context.Set<User>()?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return PartialView("_ChangePassword", new ChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<JsonResult> ChangePassword(string? id, [Bind("Password,ConfirmPassword")] ChangePasswordViewModel changePasswordVm)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, changePasswordVm.Password);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
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
    }
}
