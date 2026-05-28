using System.ComponentModel.DataAnnotations;

namespace GameVault.Models;

// Game development studio / publisher
public class Studio
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Country { get; set; } = string.Empty;

    [Display(Name = "Founded")]
    public int FoundedYear { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(200)]
    public string Website { get; set; } = string.Empty;

    // Navigation property: games developed by this studio
    public ICollection<Game> Games { get; set; } = new List<Game>();
}
