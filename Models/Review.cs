using System.ComponentModel.DataAnnotations;

namespace GameVault.Models;

// User review for a game
public class Review
{
    public int Id { get; set; }

    [Display(Name = "Game")]
    public int GameId { get; set; }

    [Required]
    [Display(Name = "Reviewer")]
    [StringLength(50)]
    public string ReviewerName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Comment { get; set; } = string.Empty;

    [Range(1, 10)]
    public int Rating { get; set; }

    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    public bool Recommended { get; set; }

    // Navigation property (nullable so MVC model validation does not treat it as required)
    public Game? Game { get; set; }
}
