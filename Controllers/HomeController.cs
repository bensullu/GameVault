using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;
using GameVault.Models;

namespace GameVault.Controllers;

public class HomeController : Controller
{
    private readonly GameVaultDbContext _context;

    public HomeController(GameVaultDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Top 4 rated games for the homepage hero
        ViewBag.FeaturedGames = await _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .Include(g => g.Reviews)
            .Where(g => g.Reviews.Any())
            .OrderByDescending(g => g.Reviews.Average(r => r.Rating))
            .ThenByDescending(g => g.Reviews.Count)
            .Take(4)
            .ToListAsync();

        // Newest 6 games
        ViewBag.NewReleases = await _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .OrderByDescending(g => g.ReleaseDate)
            .Take(6)
            .ToListAsync();

        // Quick stats for the homepage banner
        ViewBag.TotalGames = await _context.Games.CountAsync();
        ViewBag.TotalStudios = await _context.Studios.CountAsync();
        ViewBag.TotalGenres = await _context.Genres.CountAsync();
        ViewBag.TotalReviews = await _context.Reviews.CountAsync();

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
