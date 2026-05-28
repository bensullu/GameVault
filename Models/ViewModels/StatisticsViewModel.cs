namespace GameVault.Models.ViewModels;

// Statistics overview
public class StatisticsViewModel
{
    public int TotalGames { get; set; }
    public int TotalStudios { get; set; }
    public int TotalGenres { get; set; }
    public int TotalReviews { get; set; }
    public decimal AveragePrice { get; set; }
    public double AverageRating { get; set; }

    // Top-rated game (best average rating)
    public Game? TopRatedGame { get; set; }
    public double TopRatingValue { get; set; }

    // Most reviewed game
    public Game? MostReviewedGame { get; set; }
    public int MostReviewsCount { get; set; }

    // Genre breakdown: games per genre
    public List<GenreStat> GenreStats { get; set; } = new();

    // Studio breakdown: games per studio
    public List<StudioStat> StudioStats { get; set; } = new();
}

public class GenreStat
{
    public Genre Genre { get; set; } = null!;
    public int GameCount { get; set; }
    public decimal AveragePrice { get; set; }
    public double AverageRating { get; set; }
}

public class StudioStat
{
    public Studio Studio { get; set; } = null!;
    public int GameCount { get; set; }
    public double AverageRating { get; set; }
}
