using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using TaskModel = Projekt.Models.Task;

namespace Projekt.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private readonly ProjektContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TasksController(ProjektContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: Tasks
        public async Task<IActionResult> Index(string category)
        {
            var userId = _userManager.GetUserId(User);
            var tasksQuery = _context.Task
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Date)
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                tasksQuery = tasksQuery.Where(t => t.TaskCategory == category);
            }

            var tasks = await tasksQuery.ToListAsync();
            ViewBag.Categories = await _context.Task
                .Where(t => t.UserId == userId && !string.IsNullOrEmpty(t.TaskCategory))
                .Select(t => t.TaskCategory)
                .Distinct()
                .ToListAsync();

            ViewBag.SelectedCategory = category;

            return View(tasks);
        }


        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Time,Description,IsCompleted,TaskCategory")] TaskModel task)
        {
            if (ModelState.IsValid)
            {
                task.UserId = _userManager.GetUserId(User);
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }


        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }


        // POST: Tasks/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Time,Description,IsCompleted,TaskCategory,UserId")] TaskModel task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    task.UserId = _userManager.GetUserId(User);
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }


        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id && m.UserId == userId);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }


        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.Task.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task != null)
            {
                _context.Task.Remove(task);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
            var userId = _userManager.GetUserId(User);
            return _context.Task.Any(e => e.Id == id && e.UserId == userId);
        }
    }
}


