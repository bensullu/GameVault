using System.ComponentModel.DataAnnotations;

namespace GameVault.Models;

// Main entity: video game
public class Game
{
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Display(Name = "Studio")]
    public int StudioId { get; set; }

    [Display(Name = "Genre")]
    public int GenreId { get; set; }

    // Navigation properties (nullable so MVC model validation does not treat them as required)
    public Studio? Studio { get; set; }
    public Genre? Genre { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
