using System.ComponentModel.DataAnnotations;

namespace GameVault.Models;

// Game genre / category
public class Genre
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(300)]
    public string Description { get; set; } = string.Empty;

    // Navigation property: games belonging to this genre
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
