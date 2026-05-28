using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;
using GameVault.Models;

namespace GameVault.Controllers;

// Full CRUD for games + search, sort, paging
public class GamesController : Controller
{
    private readonly GameVaultDbContext _context;

    public GamesController(GameVaultDbContext context)
    {
        _context = context;
    }

    // GET: Games - list with search, sort, paging, genre filter
    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? genreId, int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["TitleSortParam"] = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
        ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
        ViewData["PriceSortParam"] = sortOrder == "price" ? "price_desc" : "price";
        ViewData["RatingSortParam"] = sortOrder == "rating" ? "rating_desc" : "rating";

        // Reset to page 1 when a new search is performed
        if (searchString != null)
            pageNumber = 1;
        else
            searchString = currentFilter;

        ViewData["CurrentFilter"] = searchString;
        ViewData["GenreFilter"] = genreId;

        // Populate genres dropdown for filter
        ViewBag.Genres = await _context.Genres.OrderBy(g => g.Name).ToListAsync();

        var games = _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .Include(g => g.Reviews)
            .AsQueryable();

        // Search by title, studio or genre name
        if (!string.IsNullOrEmpty(searchString))
        {
            var s = searchString.ToLower();
            games = games.Where(g =>
                g.Title.ToLower().Contains(s) ||
                g.Studio.Name.ToLower().Contains(s) ||
                g.Genre.Name.ToLower().Contains(s));
        }

        // Optional genre filter
        if (genreId.HasValue && genreId > 0)
            games = games.Where(g => g.GenreId == genreId.Value);

        // Sort (cast Price to double because SQLite does not support ORDER BY on decimal columns)
        games = sortOrder switch
        {
            "title_desc" => games.OrderByDescending(g => g.Title),
            "date" => games.OrderBy(g => g.ReleaseDate),
            "date_desc" => games.OrderByDescending(g => g.ReleaseDate),
            "price" => games.OrderBy(g => (double)g.Price),
            "price_desc" => games.OrderByDescending(g => (double)g.Price),
            "rating" => games.OrderBy(g => g.Reviews.Any() ? g.Reviews.Average(r => r.Rating) : 0),
            "rating_desc" => games.OrderByDescending(g => g.Reviews.Any() ? g.Reviews.Average(r => r.Rating) : 0),
            _ => games.OrderBy(g => g.Title),
        };

        int pageSize = 8;
        return View(await PaginatedList<Game>.CreateAsync(games.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    // GET: Games/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .Include(g => g.Reviews.OrderByDescending(r => r.Date))
            .Include(g => g.GamePlatforms)
                .ThenInclude(gp => gp.Platform)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();

        return View(game);
    }

    // GET: Games/Create
    public async Task<IActionResult> Create()
    {
        await PopulateDropdowns();
        return View();
    }

    // POST: Games/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Title,Description,Price,ReleaseDate,StudioId,GenreId")] Game game, int[] selectedPlatforms)
    {
        if (ModelState.IsValid)
        {
            _context.Add(game);
            await _context.SaveChangesAsync();

            // Add many-to-many platform relationships
            if (selectedPlatforms != null)
            {
                foreach (var pId in selectedPlatforms)
                    _context.GamePlatforms.Add(new GamePlatform { GameId = game.Id, PlatformId = pId });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = game.Id });
        }

        await PopulateDropdowns(game);
        return View(game);
    }

    // GET: Games/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games
            .Include(g => g.GamePlatforms)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();

        await PopulateDropdowns(game);
        ViewBag.SelectedPlatforms = game.GamePlatforms.Select(gp => gp.PlatformId).ToList();
        return View(game);
    }

    // POST: Games/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ReleaseDate,StudioId,GenreId")] Game game, int[] selectedPlatforms)
    {
        if (id != game.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(game);

                // Refresh the many-to-many platform relationships
                var existing = _context.GamePlatforms.Where(gp => gp.GameId == id);
                _context.GamePlatforms.RemoveRange(existing);
                if (selectedPlatforms != null)
                {
                    foreach (var pId in selectedPlatforms)
                        _context.GamePlatforms.Add(new GamePlatform { GameId = id, PlatformId = pId });
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Games.AnyAsync(g => g.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Details), new { id = game.Id });
        }

        await PopulateDropdowns(game);
        ViewBag.SelectedPlatforms = selectedPlatforms?.ToList() ?? new List<int>();
        return View(game);
    }

    // GET: Games/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var game = await _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (game == null) return NotFound();

        return View(game);
    }

    // POST: Games/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var game = await _context.Games.FindAsync(id);
        if (game != null)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    // Populate Studio, Genre, and Platform dropdowns for forms
    private async Task PopulateDropdowns(Game? game = null)
    {
        ViewData["StudioId"] = new SelectList(await _context.Studios.OrderBy(s => s.Name).ToListAsync(), "Id", "Name", game?.StudioId);
        ViewData["GenreId"] = new SelectList(await _context.Genres.OrderBy(g => g.Name).ToListAsync(), "Id", "Name", game?.GenreId);
        ViewBag.Platforms = await _context.Platforms.OrderBy(p => p.Name).ToListAsync();
    }
}
