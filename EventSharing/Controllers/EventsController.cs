using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventSharing.Data;
using EventSharing.Models;
using AutoMapper;
using EventSharing.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace EventSharing.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public EventsController(ApplicationDbContext context, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Events
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Index()
        {

            List<Event> events;
            var currentUser = await _userManager.GetUserAsync(User);
            bool isOrganizer = await _userManager.IsInRoleAsync(currentUser, "Organizer");
            if (isOrganizer)
            {
                events = await _context.Events
                    .Include(e => e.Category)
                    .Where(e => e.Creator.Email.Equals(User.FindFirstValue(ClaimTypes.Email)))
                    .ToListAsync();
            }
            else
            {

                events = await _context.Events
                        .Include(e => e.Category)
                        .ToListAsync();
            }

            return _context.Events != null ? 
                          View(_mapper.Map<List<EventViewModel>>(events)) :
                          Problem("Entity set 'ApplicationDbContext.Events'  is null.");
        }

        // GET: Events/Details/5
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var eventVm = _mapper.Map<EventViewModel>(await _context.Events
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id));
            if (eventVm == null)
            {
                return NotFound();
            }

            return View(eventVm);
        }

        // GET: Events/Create
        [Authorize(Roles = "Organizer,Admin")]
        public IActionResult Create()
        {
            var eventVm = new EventViewModel
            {
                CategoriesVm = _mapper.Map<List<CategoryViewModel>>(_context.Categories.ToList()),
            };
            return View(eventVm);
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,IdCategory")] EventViewModel eventVm)
        {
            if (ModelState.IsValid)
            {
                var @event = _mapper.Map<Event>(eventVm);
                @event.Category = _context.Categories
                    .FirstOrDefault(c => c.Id.Equals(eventVm.IdCategory));
                @event.Creator = _context.Set<User>()
                    .FirstOrDefault(o => o.Email.Equals(User.FindFirstValue(ClaimTypes.Email)));
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            eventVm.CategoriesVm = _mapper.Map<List<CategoryViewModel>>(_context.Categories.ToList());
            return View(eventVm);
        }

        // GET: Events/Edit/5
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var eventVm = _mapper.Map<EventViewModel>(await _context.Events
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id));
            if (eventVm == null)
            {
                return NotFound();
            }
            eventVm.CategoriesVm = _mapper.Map<List<CategoryViewModel>>(_context.Categories.ToList());
            return View(eventVm);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Organizer,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,IdCategory")] EventViewModel eventVm)
        {
            if (id != eventVm.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var @event = _mapper.Map<Event>(eventVm);
                    @event.Category = _context.Categories
                        .FirstOrDefault(c => c.Id.Equals(eventVm.IdCategory));
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventVm.Id))
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
            eventVm.CategoriesVm = _mapper.Map<List<CategoryViewModel>>(_context.Categories.ToList());
            return View(eventVm);
        }

        // GET: Events/Delete/5
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Events == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(_mapper.Map<EventViewModel>(@event));
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Events == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Events'  is null.");
            }
            var @event = await _context.Events.FindAsync(id);
            if (@event != null)
            {
                _context.Events.Remove(@event);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
          return (_context.Events?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize]
        public IActionResult Search()
        {
            var eventSearchVm = new EventSearchViewModel
            {
                CategoriesVm = _mapper.Map<List<CategoryViewModel>>(_context.Categories.ToList()),
            };
            return View(eventSearchVm);
        }

        public async Task<JsonResult> GetSearchResults(EventSearchViewModel eventSearchVm)
        {
            var foundEvents = await _context.Events
              .Where(e => (e.Name.Contains(eventSearchVm.SearchTerm)
              || e.Description.Contains(eventSearchVm.SearchTerm))
              && e.StartDate >= eventSearchVm.StartDate
              && e.Category.Id == eventSearchVm.IdCategory)
              .ToListAsync();

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            var eventsUserIsRegistredFor = _context.Set<User>()
                .Include(u => u.JoinedEvents)
                .FirstOrDefault(e => e.Email == userEmail)
                .JoinedEvents
                .Where(e => foundEvents.Any(fe => fe.Id == e.Id))
                .Select(e => e.Id)
                .ToList();

            eventSearchVm.EventsVm = _mapper.Map<List<EventViewModel>>(foundEvents);

            foreach (var eventVm in eventSearchVm.EventsVm)
            {
                eventVm.IsCurrentUserRegistred = eventsUserIsRegistredFor.Contains(eventVm.Id);
            }

            return Json(eventSearchVm);
                
        }

        public async Task<IActionResult> ManageRegistration(int? eventId, bool register)
        {
            var currentEvent = await _context.Events
                .Include(e => e.Participants)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (currentEvent != null)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var currentUser = await _context.Set<User>()
                    .SingleOrDefaultAsync(u => u.Email == userEmail);

                if (register)
                    currentEvent.Participants.Add(currentUser);
                else
                    currentEvent.Participants.Remove(currentUser);

                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            else
            {
                return NotFound(
                    new
                    {
                        success = false,
                        errorMessage = "L'activité non trouvée"
                    }
                  );
            }
        }
    }
}
