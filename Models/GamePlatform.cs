namespace GameVault.Models;

// Junction table: many-to-many between Game and Platform
public class GamePlatform
{
    public int GameId { get; set; }
    public int PlatformId { get; set; }

    // Navigation properties (nullable so MVC model validation does not treat them as required)
    public Game? Game { get; set; }
    public Platform? Platform { get; set; }
}
