using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;
using GameVault.Models.ViewModels;

namespace GameVault.Controllers;

// Statistics page: library-wide stats, top-rated games, genre breakdown
public class StatisticsController : Controller
{
    private readonly GameVaultDbContext _context;

    public StatisticsController(GameVaultDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var vm = new StatisticsViewModel();

        // Load games with related data for LINQ aggregation
        var games = await _context.Games
            .Include(g => g.Studio)
            .Include(g => g.Genre)
            .Include(g => g.Reviews)
            .ToListAsync();

        var reviews = await _context.Reviews.ToListAsync();

        // General counts
        vm.TotalGames = games.Count;
        vm.TotalStudios = await _context.Studios.CountAsync();
        vm.TotalGenres = await _context.Genres.CountAsync();
        vm.TotalReviews = reviews.Count;

        // Average price and rating across the library
        vm.AveragePrice = games.Any() ? Math.Round(games.Average(g => g.Price), 2) : 0;
        vm.AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 2) : 0;

        // Find the top-rated game (by average rating)
        var ratedGames = games.Where(g => g.Reviews.Any()).ToList();
        if (ratedGames.Any())
        {
            var top = ratedGames.OrderByDescending(g => g.Reviews.Average(r => r.Rating)).First();
            vm.TopRatedGame = top;
            vm.TopRatingValue = Math.Round(top.Reviews.Average(r => r.Rating), 2);

            // Most reviewed game
            var mostReviewed = games.OrderByDescending(g => g.Reviews.Count).First();
            vm.MostReviewedGame = mostReviewed;
            vm.MostReviewsCount = mostReviewed.Reviews.Count;
        }

        // Genre breakdown: count, avg price, avg rating per genre
        var genres = await _context.Genres
            .Include(g => g.Games)
                .ThenInclude(game => game.Reviews)
            .ToListAsync();

        vm.GenreStats = genres
            .Select(g => new GenreStat
            {
                Genre = g,
                GameCount = g.Games.Count,
                AveragePrice = g.Games.Any() ? Math.Round(g.Games.Average(game => game.Price), 2) : 0,
                AverageRating = g.Games.SelectMany(game => game.Reviews).Any()
                    ? Math.Round(g.Games.SelectMany(game => game.Reviews).Average(r => r.Rating), 2)
                    : 0
            })
            .OrderByDescending(gs => gs.GameCount)
            .ToList();

        // Studio breakdown
        var studios = await _context.Studios
            .Include(s => s.Games)
                .ThenInclude(game => game.Reviews)
            .ToListAsync();

        vm.StudioStats = studios
            .Select(s => new StudioStat
            {
                Studio = s,
                GameCount = s.Games.Count,
                AverageRating = s.Games.SelectMany(game => game.Reviews).Any()
                    ? Math.Round(s.Games.SelectMany(game => game.Reviews).Average(r => r.Rating), 2)
                    : 0
            })
            .OrderByDescending(ss => ss.GameCount)
            .ThenByDescending(ss => ss.AverageRating)
            .ToList();

        return View(vm);
    }
}
