using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Projekt.Data;
using Projekt.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly ProjektContext _context;

        public StatisticsController(ProjektContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;

            var lastWeek = DateTime.Now.AddDays(-7);
            var lastMonth = DateTime.Now.AddMonths(-1);
            var lastYear = DateTime.Now.AddYears(-1);

            var tasksLastWeek = await _context.Task
                .Where(t => t.UserId == userId && t.Date >= lastWeek)
                .ToListAsync();

            var tasksLastMonth = await _context.Task
                .Where(t => t.UserId == userId && t.Date >= lastMonth)
                .ToListAsync();

            var tasksLastYear = await _context.Task
                .Where(t => t.UserId == userId && t.Date >= lastYear)
                .ToListAsync();

            var completedTasksLastWeek = tasksLastWeek.Count(t => t.IsCompleted);
            var uncompletedTasksLastWeek = tasksLastWeek.Count(t => !t.IsCompleted);

            var completedTasksLastMonth = tasksLastMonth.Count(t => t.IsCompleted);
            var uncompletedTasksLastMonth = tasksLastMonth.Count(t => !t.IsCompleted);

            var completedTasksLastYear = tasksLastYear.Count(t => t.IsCompleted);
            var uncompletedTasksLastYear = tasksLastYear.Count(t => !t.IsCompleted);

            var categoryCounts = await _context.Task
                .Where(t => t.UserId == userId)
                .GroupBy(t => t.TaskCategory)
                .Select(g => new CategoryCount
                {
                    Category = g.Key,
                    Count = g.Count(),
                    CompletedCount = g.Count(t => t.IsCompleted)
                })
                .ToListAsync();

            // Calculate efficiencies
            var totalTasksLastWeek = completedTasksLastWeek + uncompletedTasksLastWeek;
            var totalTasksLastMonth = completedTasksLastMonth + uncompletedTasksLastMonth;
            var totalTasksLastYear = completedTasksLastYear + uncompletedTasksLastYear;

            var efficiencyLastWeek = totalTasksLastWeek > 0
                ? (double)completedTasksLastWeek / totalTasksLastWeek * 100
                : 0;

            var efficiencyLastMonth = totalTasksLastMonth > 0
                ? (double)completedTasksLastMonth / totalTasksLastMonth * 100
                : 0;

            var efficiencyLastYear = totalTasksLastYear > 0
                ? (double)completedTasksLastYear / totalTasksLastYear * 100
                : 0;

            foreach (var category in categoryCounts)
            {
                category.Efficiency = category.Count > 0
                    ? (double)category.CompletedCount / category.Count * 100
                    : 0;
            }

            var model = new Statistic
            {
                CompletedTasksLastWeek = completedTasksLastWeek,
                UncompletedTasksLastWeek = uncompletedTasksLastWeek,
                CompletedTasksLastMonth = completedTasksLastMonth,
                UncompletedTasksLastMonth = uncompletedTasksLastMonth,
                CompletedTasksLastYear = completedTasksLastYear,
                UncompletedTasksLastYear = uncompletedTasksLastYear,
                CategoryCounts = categoryCounts,
                EfficiencyLastWeek = efficiencyLastWeek,
                EfficiencyLastMonth = efficiencyLastMonth,
                EfficiencyLastYear = efficiencyLastYear
            };

            return View(model);
        }
    }
}
