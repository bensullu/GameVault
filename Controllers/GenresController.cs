using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;

namespace GameVault.Controllers;

public class GenresController : Controller
{
    private readonly GameVaultDbContext _context;

    public GenresController(GameVaultDbContext context)
    {
        _context = context;
    }

    // GET: Genres - browse all genres
    public async Task<IActionResult> Index()
    {
        var genres = await _context.Genres
            .Include(g => g.Games)
            .OrderBy(g => g.Name)
            .ToListAsync();

        return View(genres);
    }

    // GET: Genres/Details/5 - show genre with its games
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var genre = await _context.Genres
            .Include(g => g.Games)
                .ThenInclude(game => game.Studio)
            .Include(g => g.Games)
                .ThenInclude(game => game.Reviews)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (genre == null) return NotFound();

        return View(genre);
    }
}
