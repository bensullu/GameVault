using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GameVault.Data;
using GameVault.Models;

namespace GameVault.Controllers;

// Reviews CRUD (write/edit/delete reviews for games)
public class ReviewsController : Controller
{
    private readonly GameVaultDbContext _context;

    public ReviewsController(GameVaultDbContext context)
    {
        _context = context;
    }

    // GET: Reviews/Create?gameId=5 - write a review for a specific game
    public async Task<IActionResult> Create(int? gameId)
    {
        ViewBag.Games = new SelectList(await _context.Games.OrderBy(g => g.Title).ToListAsync(), "Id", "Title", gameId);
        var review = new Review { GameId = gameId ?? 0, Date = DateTime.Today, Recommended = true, Rating = 8 };
        return View(review);
    }

    // POST: Reviews/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("GameId,ReviewerName,Title,Comment,Rating,Date,Recommended")] Review review)
    {
        if (ModelState.IsValid)
        {
            _context.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Games", new { id = review.GameId });
        }
        ViewBag.Games = new SelectList(await _context.Games.OrderBy(g => g.Title).ToListAsync(), "Id", "Title", review.GameId);
        return View(review);
    }

    // GET: Reviews/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var review = await _context.Reviews.FindAsync(id);
        if (review == null) return NotFound();

        ViewBag.Games = new SelectList(await _context.Games.OrderBy(g => g.Title).ToListAsync(), "Id", "Title", review.GameId);
        return View(review);
    }

    // POST: Reviews/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,GameId,ReviewerName,Title,Comment,Rating,Date,Recommended")] Review review)
    {
        if (id != review.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(review);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Reviews.AnyAsync(r => r.Id == id))
                    return NotFound();
                throw;
            }
            return RedirectToAction("Details", "Games", new { id = review.GameId });
        }

        ViewBag.Games = new SelectList(await _context.Games.OrderBy(g => g.Title).ToListAsync(), "Id", "Title", review.GameId);
        return View(review);
    }

    // GET: Reviews/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var review = await _context.Reviews
            .Include(r => r.Game)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null) return NotFound();

        return View(review);
    }

    // POST: Reviews/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var review = await _context.Reviews.FindAsync(id);
        int? gameId = review?.GameId;

        if (review != null)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }

        if (gameId != null)
            return RedirectToAction("Details", "Games", new { id = gameId });

        return RedirectToAction("Index", "Games");
    }
}
