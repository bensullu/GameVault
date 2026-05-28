using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;

namespace GameVault.Controllers;

public class PlatformsController : Controller
{
    private readonly GameVaultDbContext _context;

    public PlatformsController(GameVaultDbContext context)
    {
        _context = context;
    }

    // GET: Platforms - list all platforms with game counts
    public async Task<IActionResult> Index()
    {
        var platforms = await _context.Platforms
            .Include(p => p.GamePlatforms)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return View(platforms);
    }

    // GET: Platforms/Details/5 - show games available on this platform
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var platform = await _context.Platforms
            .Include(p => p.GamePlatforms)
                .ThenInclude(gp => gp.Game)
                    .ThenInclude(g => g.Genre)
            .Include(p => p.GamePlatforms)
                .ThenInclude(gp => gp.Game)
                    .ThenInclude(g => g.Studio)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (platform == null) return NotFound();

        return View(platform);
    }
}
