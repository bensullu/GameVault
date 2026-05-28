using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;

namespace GameVault.Controllers;

public class StudiosController : Controller
{
    private readonly GameVaultDbContext _context;

    public StudiosController(GameVaultDbContext context)
    {
        _context = context;
    }

    // GET: Studios - list all studios with game count
    public async Task<IActionResult> Index()
    {
        var studios = await _context.Studios
            .Include(s => s.Games)
            .OrderBy(s => s.Name)
            .ToListAsync();

        return View(studios);
    }

    // GET: Studios/Details/5 - show studio with its games
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var studio = await _context.Studios
            .Include(s => s.Games)
                .ThenInclude(g => g.Genre)
            .Include(s => s.Games)
                .ThenInclude(g => g.Reviews)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (studio == null) return NotFound();

        return View(studio);
    }
}
