using System.ComponentModel.DataAnnotations;

namespace GameVault.Models;

// Gaming platform (PC, console, handheld)
public class Platform
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Manufacturer { get; set; } = string.Empty;

    [Display(Name = "Released")]
    public int ReleaseYear { get; set; }

    // Navigation property: link to games available on this platform
    public ICollection<GamePlatform> GamePlatforms { get; set; } = new List<GamePlatform>();
}
