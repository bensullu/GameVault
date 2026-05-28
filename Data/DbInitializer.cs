using GameVault.Models;

namespace GameVault.Data;

public static class DbInitializer
{
    public static void Initialize(GameVaultDbContext context)
    {
        context.Database.EnsureCreated();

        // Skip if data already seeded
        if (context.Games.Any()) return;

        // === GENRES ===
        var genres = new Genre[]
        {
            new() { Name = "Action", Description = "Fast-paced gameplay with combat and reflexes" },
            new() { Name = "RPG", Description = "Role-playing games with story and character progression" },
            new() { Name = "FPS", Description = "First-person shooter games" },
            new() { Name = "Adventure", Description = "Exploration and narrative-driven games" },
            new() { Name = "Strategy", Description = "Tactical and strategic gameplay" },
            new() { Name = "Racing", Description = "Vehicle racing and driving simulation" },
            new() { Name = "Indie", Description = "Independent developer games" },
            new() { Name = "Horror", Description = "Scary and suspenseful experiences" },
            new() { Name = "Open World", Description = "Large explorable game worlds" },
            new() { Name = "Survival", Description = "Survive against hostile environments" },
        };
        context.Genres.AddRange(genres);
        context.SaveChanges();

        // === PLATFORMS ===
        var platforms = new Platform[]
        {
            new() { Name = "PC", Manufacturer = "Various", ReleaseYear = 1981 },
            new() { Name = "PlayStation 5", Manufacturer = "Sony", ReleaseYear = 2020 },
            new() { Name = "Xbox Series X", Manufacturer = "Microsoft", ReleaseYear = 2020 },
            new() { Name = "Nintendo Switch", Manufacturer = "Nintendo", ReleaseYear = 2017 },
            new() { Name = "Steam Deck", Manufacturer = "Valve", ReleaseYear = 2022 },
        };
        context.Platforms.AddRange(platforms);
        context.SaveChanges();

        // === STUDIOS ===
        var studios = new Studio[]
        {
            new() { Name = "Valve", Country = "United States", FoundedYear = 1996, Description = "Creators of Steam and groundbreaking PC games", Website = "valvesoftware.com" },
            new() { Name = "CD Projekt Red", Country = "Poland", FoundedYear = 1994, Description = "Polish studio known for narrative RPGs", Website = "cdprojektred.com" },
            new() { Name = "Rockstar Games", Country = "United States", FoundedYear = 1998, Description = "Developer of GTA and Red Dead series", Website = "rockstargames.com" },
            new() { Name = "FromSoftware", Country = "Japan", FoundedYear = 1986, Description = "Masters of the soulslike genre", Website = "fromsoftware.jp" },
            new() { Name = "Naughty Dog", Country = "United States", FoundedYear = 1984, Description = "Sony first-party studio behind Uncharted and TLoU", Website = "naughtydog.com" },
            new() { Name = "Bethesda Game Studios", Country = "United States", FoundedYear = 2001, Description = "Creators of Skyrim and Fallout", Website = "bethesda.net" },
            new() { Name = "Mojang Studios", Country = "Sweden", FoundedYear = 2009, Description = "Developer of Minecraft", Website = "mojang.com" },
            new() { Name = "Square Enix", Country = "Japan", FoundedYear = 1986, Description = "Iconic JRPG publisher and developer", Website = "square-enix.com" },
            new() { Name = "Nintendo EPD", Country = "Japan", FoundedYear = 2015, Description = "Nintendo's main internal development team", Website = "nintendo.com" },
            new() { Name = "Ubisoft Montreal", Country = "Canada", FoundedYear = 1997, Description = "Creators of Assassin's Creed and Far Cry", Website = "ubisoft.com" },
            new() { Name = "Supergiant Games", Country = "United States", FoundedYear = 2009, Description = "Award-winning indie studio", Website = "supergiantgames.com" },
            new() { Name = "Capcom", Country = "Japan", FoundedYear = 1979, Description = "Japanese publisher behind Resident Evil and Monster Hunter", Website = "capcom.com" },
        };
        context.Studios.AddRange(studios);
        context.SaveChanges();

        // Genre lookup shortcuts: Action(0), RPG(1), FPS(2), Adventure(3), Strategy(4), Racing(5), Indie(6), Horror(7), Open World(8), Survival(9)
        // Studio lookup shortcuts: Valve(0), CDPR(1), Rockstar(2), FromSoft(3), NaughtyDog(4), Bethesda(5), Mojang(6), SquareEnix(7), Nintendo(8), Ubisoft(9), Supergiant(10), Capcom(11)

        // === GAMES ===
        var games = new Game[]
        {
            new() { Title = "The Witcher 3: Wild Hunt", Description = "An open-world action RPG following Geralt of Rivia, a monster hunter searching for his adopted daughter in a war-torn fantasy world.", Price = 39.99m, ReleaseDate = new DateTime(2015, 5, 19), StudioId = studios[1].Id, GenreId = genres[1].Id },
            new() { Title = "Elden Ring", Description = "An action RPG set in the Lands Between, created in collaboration with George R.R. Martin. Explore a vast open world filled with deadly enemies.", Price = 59.99m, ReleaseDate = new DateTime(2022, 2, 25), StudioId = studios[3].Id, GenreId = genres[1].Id },
            new() { Title = "Cyberpunk 2077", Description = "An open-world action-adventure RPG set in Night City, a megalopolis obsessed with power, glamour and body modification.", Price = 59.99m, ReleaseDate = new DateTime(2020, 12, 10), StudioId = studios[1].Id, GenreId = genres[1].Id },
            new() { Title = "Red Dead Redemption 2", Description = "Epic tale of life in America's unforgiving heartland. Follow outlaw Arthur Morgan and the Van der Linde gang in 1899.", Price = 59.99m, ReleaseDate = new DateTime(2018, 10, 26), StudioId = studios[2].Id, GenreId = genres[0].Id },
            new() { Title = "Grand Theft Auto V", Description = "Three very different criminals risk everything in a series of daring heists in the sprawling city of Los Santos.", Price = 29.99m, ReleaseDate = new DateTime(2013, 9, 17), StudioId = studios[2].Id, GenreId = genres[8].Id },
            new() { Title = "The Last of Us Part II", Description = "Five years after their journey, Ellie must navigate a path of vengeance in a post-apocalyptic United States.", Price = 39.99m, ReleaseDate = new DateTime(2020, 6, 19), StudioId = studios[4].Id, GenreId = genres[0].Id },
            new() { Title = "The Elder Scrolls V: Skyrim", Description = "Become the Dragonborn and explore the vast lands of Skyrim. Slay dragons, master magic, and shape your destiny.", Price = 39.99m, ReleaseDate = new DateTime(2011, 11, 11), StudioId = studios[5].Id, GenreId = genres[1].Id },
            new() { Title = "Minecraft", Description = "A sandbox game where players explore a blocky, procedurally generated world, gather resources, craft tools, and build anything imaginable.", Price = 26.95m, ReleaseDate = new DateTime(2011, 11, 18), StudioId = studios[6].Id, GenreId = genres[9].Id },
            new() { Title = "Half-Life 2", Description = "Gordon Freeman returns to fight the alien Combine forces oppressing Earth in this revolutionary first-person shooter.", Price = 9.99m, ReleaseDate = new DateTime(2004, 11, 16), StudioId = studios[0].Id, GenreId = genres[2].Id },
            new() { Title = "Portal 2", Description = "Solve mind-bending puzzles using a portal gun while being taunted by a sarcastic AI. A masterpiece of game design.", Price = 9.99m, ReleaseDate = new DateTime(2011, 4, 19), StudioId = studios[0].Id, GenreId = genres[3].Id },
            new() { Title = "Counter-Strike 2", Description = "The legendary competitive FPS reborn on the Source 2 engine. Tactical 5v5 multiplayer combat.", Price = 0.00m, ReleaseDate = new DateTime(2023, 9, 27), StudioId = studios[0].Id, GenreId = genres[2].Id },
            new() { Title = "Dark Souls III", Description = "The final chapter in the legendary Dark Souls series. Punishing combat in a dying world of decay and ash.", Price = 59.99m, ReleaseDate = new DateTime(2016, 4, 12), StudioId = studios[3].Id, GenreId = genres[1].Id },
            new() { Title = "Sekiro: Shadows Die Twice", Description = "A reimagining of the Soulslike genre with a focus on swordplay and stealth in feudal Japan.", Price = 59.99m, ReleaseDate = new DateTime(2019, 3, 22), StudioId = studios[3].Id, GenreId = genres[0].Id },
            new() { Title = "Uncharted 4: A Thief's End", Description = "Nathan Drake's final adventure takes him on a globe-trotting journey in search of a legendary pirate treasure.", Price = 29.99m, ReleaseDate = new DateTime(2016, 5, 10), StudioId = studios[4].Id, GenreId = genres[3].Id },
            new() { Title = "Hades", Description = "A roguelike dungeon crawler where you defy the god of the dead as you hack and slash out of the Underworld.", Price = 24.99m, ReleaseDate = new DateTime(2020, 9, 17), StudioId = studios[10].Id, GenreId = genres[6].Id },
            new() { Title = "Assassin's Creed Valhalla", Description = "Become Eivor, a legendary Viking raider, and lead your clan from icy Norway to a new home in 9th-century England.", Price = 59.99m, ReleaseDate = new DateTime(2020, 11, 10), StudioId = studios[9].Id, GenreId = genres[0].Id },
            new() { Title = "Final Fantasy VII Remake", Description = "A stunning reimagining of the iconic JRPG. Follow Cloud Strife and the eco-terrorist group AVALANCHE.", Price = 59.99m, ReleaseDate = new DateTime(2020, 4, 10), StudioId = studios[7].Id, GenreId = genres[1].Id },
            new() { Title = "The Legend of Zelda: Breath of the Wild", Description = "Step into a world of discovery, exploration, and adventure in this open-air Zelda game. Climb anything, explore anywhere.", Price = 59.99m, ReleaseDate = new DateTime(2017, 3, 3), StudioId = studios[8].Id, GenreId = genres[3].Id },
            new() { Title = "Mario Kart 8 Deluxe", Description = "Race against friends and family in the definitive Mario Kart experience with all-new tracks and characters.", Price = 59.99m, ReleaseDate = new DateTime(2017, 4, 28), StudioId = studios[8].Id, GenreId = genres[5].Id },
            new() { Title = "Resident Evil Village", Description = "Ethan Winters fights to survive in a mysterious Eastern European village stalked by Lady Dimitrescu and other horrors.", Price = 39.99m, ReleaseDate = new DateTime(2021, 5, 7), StudioId = studios[11].Id, GenreId = genres[7].Id },
            new() { Title = "Monster Hunter: World", Description = "Hunt giant monsters in living, breathing ecosystems across diverse environments. Craft gear from your defeated prey.", Price = 29.99m, ReleaseDate = new DateTime(2018, 1, 26), StudioId = studios[11].Id, GenreId = genres[0].Id },
            new() { Title = "Fallout 4", Description = "Emerge from Vault 111 into post-apocalyptic Boston. Build settlements, craft weapons, and shape the Commonwealth.", Price = 19.99m, ReleaseDate = new DateTime(2015, 11, 10), StudioId = studios[5].Id, GenreId = genres[8].Id },
            new() { Title = "Far Cry 6", Description = "Fight as guerilla soldier Dani Rojas to liberate the tropical paradise of Yara from a tyrannical dictator.", Price = 59.99m, ReleaseDate = new DateTime(2021, 10, 7), StudioId = studios[9].Id, GenreId = genres[2].Id },
            new() { Title = "Hades II", Description = "The sequel to the award-winning roguelike. Play as MelinoÃ«, the immortal Princess of the Underworld, on a mission of vengeance.", Price = 29.99m, ReleaseDate = new DateTime(2024, 5, 6), StudioId = studios[10].Id, GenreId = genres[6].Id },
        };
        context.Games.AddRange(games);
        context.SaveChanges();

        // === GAME-PLATFORM RELATIONSHIPS ===
        // Platform indexes: PC(0), PS5(1), Xbox(2), Switch(3), SteamDeck(4)
        var gamePlatforms = new List<GamePlatform>();

        // Helper to add game to platforms
        void AddPlatforms(int gameIdx, params int[] platformIndexes)
        {
            foreach (var p in platformIndexes)
                gamePlatforms.Add(new GamePlatform { GameId = games[gameIdx].Id, PlatformId = platforms[p].Id });
        }

        AddPlatforms(0, 0, 1, 2, 3, 4);   // Witcher 3 - all
        AddPlatforms(1, 0, 1, 2, 4);      // Elden Ring
        AddPlatforms(2, 0, 1, 2, 4);      // Cyberpunk 2077
        AddPlatforms(3, 0, 1, 2, 4);      // RDR2
        AddPlatforms(4, 0, 1, 2, 4);      // GTA V
        AddPlatforms(5, 1);                // TLoU 2 - PS5 only
        AddPlatforms(6, 0, 1, 2, 3, 4);   // Skyrim - all
        AddPlatforms(7, 0, 1, 2, 3, 4);   // Minecraft - all
        AddPlatforms(8, 0, 4);             // Half-Life 2
        AddPlatforms(9, 0, 4);             // Portal 2
        AddPlatforms(10, 0, 4);            // Counter-Strike 2
        AddPlatforms(11, 0, 1, 2, 4);     // Dark Souls 3
        AddPlatforms(12, 0, 1, 2, 4);     // Sekiro
        AddPlatforms(13, 0, 1, 4);         // Uncharted 4
        AddPlatforms(14, 0, 1, 2, 3, 4);  // Hades
        AddPlatforms(15, 0, 1, 2, 4);     // AC Valhalla
        AddPlatforms(16, 0, 1, 4);         // FF VII Remake
        AddPlatforms(17, 3);               // BOTW - Switch only
        AddPlatforms(18, 3);               // Mario Kart 8 - Switch only
        AddPlatforms(19, 0, 1, 2, 4);     // RE Village
        AddPlatforms(20, 0, 1, 2, 4);     // MH World
        AddPlatforms(21, 0, 1, 2, 4);     // Fallout 4
        AddPlatforms(22, 0, 1, 2, 4);     // Far Cry 6
        AddPlatforms(23, 0, 3, 4);         // Hades II

        context.GamePlatforms.AddRange(gamePlatforms);
        context.SaveChanges();

        // === REVIEWS ===
        var reviews = new List<Review>
        {
            // Witcher 3 (game 0)
            new() { GameId = games[0].Id, ReviewerName = "GerlatLover99", Title = "Masterpiece", Comment = "Best RPG ever made. The story is incredible and the world is so detailed.", Rating = 10, Date = new DateTime(2023, 5, 12), Recommended = true },
            new() { GameId = games[0].Id, ReviewerName = "RPGFanatic", Title = "Amazing story and gameplay", Comment = "Hundreds of hours and still finding new things. A must play.", Rating = 10, Date = new DateTime(2024, 1, 8), Recommended = true },
            new() { GameId = games[0].Id, ReviewerName = "CasualGamer", Title = "Great but long", Comment = "Fantastic game, but it takes forever to complete. Be ready for a commitment.", Rating = 9, Date = new DateTime(2024, 3, 14), Recommended = true },

            // Elden Ring (game 1)
            new() { GameId = games[1].Id, ReviewerName = "TarnishedSoul", Title = "GOTY for a reason", Comment = "FromSoftware nailed it. The open world Souls formula works perfectly.", Rating = 10, Date = new DateTime(2023, 8, 22), Recommended = true },
            new() { GameId = games[1].Id, ReviewerName = "MalikethSlayer", Title = "Brutal but fair", Comment = "Hardest game I have played and the most rewarding.", Rating = 9, Date = new DateTime(2024, 2, 5), Recommended = true },

            // Cyberpunk 2077 (game 2)
            new() { GameId = games[2].Id, ReviewerName = "NightCityV", Title = "Fixed after updates", Comment = "Rough launch but now it is incredible. Phantom Liberty DLC is amazing.", Rating = 9, Date = new DateTime(2024, 1, 22), Recommended = true },
            new() { GameId = games[2].Id, ReviewerName = "ChromedUp", Title = "Style over substance", Comment = "Looks gorgeous but the story is shorter than expected.", Rating = 7, Date = new DateTime(2023, 11, 30), Recommended = true },

            // RDR2 (game 3)
            new() { GameId = games[3].Id, ReviewerName = "ArthurMorganFan", Title = "Cinematic perfection", Comment = "Best storytelling in video games. Arthur Morgan is unforgettable.", Rating = 10, Date = new DateTime(2023, 6, 18), Recommended = true },
            new() { GameId = games[3].Id, ReviewerName = "WildWestLover", Title = "Slow but rewarding", Comment = "Pacing is slow at start but pays off with one of the best endings ever.", Rating = 10, Date = new DateTime(2024, 2, 11), Recommended = true },

            // GTA V (game 4)
            new() { GameId = games[4].Id, ReviewerName = "LosSantosLife", Title = "Still great after 10+ years", Comment = "Holds up remarkably well. Online mode keeps it fresh.", Rating = 9, Date = new DateTime(2024, 4, 1), Recommended = true },

            // TLoU 2 (game 5)
            new() { GameId = games[5].Id, ReviewerName = "EllieMain", Title = "Emotionally devastating", Comment = "Made me feel things no other game has. Controversial but brave.", Rating = 9, Date = new DateTime(2023, 9, 12), Recommended = true },

            // Skyrim (game 6)
            new() { GameId = games[6].Id, ReviewerName = "DragonbornForever", Title = "Timeless classic", Comment = "Mods make this game infinite. Bethesda magic at its peak.", Rating = 10, Date = new DateTime(2023, 7, 4), Recommended = true },
            new() { GameId = games[6].Id, ReviewerName = "ArrowToTheKnee", Title = "Bugs but charming", Comment = "Still buggy after a decade but the charm and freedom are unmatched.", Rating = 8, Date = new DateTime(2024, 3, 19), Recommended = true },

            // Minecraft (game 7)
            new() { GameId = games[7].Id, ReviewerName = "BlockBuilder", Title = "Infinite creativity", Comment = "Best sandbox ever. Great for kids and adults.", Rating = 10, Date = new DateTime(2024, 4, 15), Recommended = true },

            // Half-Life 2 (game 8)
            new() { GameId = games[8].Id, ReviewerName = "FreemanFan", Title = "Revolutionary FPS", Comment = "Changed gaming forever. Holds up surprisingly well.", Rating = 10, Date = new DateTime(2023, 12, 5), Recommended = true },

            // Portal 2 (game 9)
            new() { GameId = games[9].Id, ReviewerName = "CakeIsALie", Title = "Perfect puzzle game", Comment = "GLaDOS is the best villain. Hilarious writing and clever puzzles.", Rating = 10, Date = new DateTime(2024, 1, 30), Recommended = true },

            // CS2 (game 10)
            new() { GameId = games[10].Id, ReviewerName = "HeadshotKing", Title = "Same old CS but prettier", Comment = "Source 2 upgrade is nice. Still the king of competitive shooters.", Rating = 8, Date = new DateTime(2024, 5, 1), Recommended = true },

            // Dark Souls 3 (game 11)
            new() { GameId = games[11].Id, ReviewerName = "PraiseTheSun", Title = "Greatest farewell", Comment = "Perfect ending to the trilogy. Difficult but always fair.", Rating = 9, Date = new DateTime(2023, 10, 14), Recommended = true },

            // Sekiro (game 12)
            new() { GameId = games[12].Id, ReviewerName = "ShinobiMaster", Title = "Combat perfection", Comment = "The most satisfying combat in any game. Steep learning curve.", Rating = 9, Date = new DateTime(2024, 2, 28), Recommended = true },

            // Hades (game 14)
            new() { GameId = games[14].Id, ReviewerName = "RogueLikeQueen", Title = "Perfect indie game", Comment = "Story progression in a roguelike that actually works. Genius design.", Rating = 10, Date = new DateTime(2024, 1, 12), Recommended = true },

            // Zelda BOTW (game 17)
            new() { GameId = games[17].Id, ReviewerName = "HyruleHero", Title = "Open world done right", Comment = "Climb anything, explore anywhere. Game-changing design.", Rating = 10, Date = new DateTime(2023, 8, 30), Recommended = true },

            // RE Village (game 19)
            new() { GameId = games[19].Id, ReviewerName = "HorrorJunkie", Title = "Lady Dimitrescu was worth it", Comment = "Great horror with memorable villains. A bit short though.", Rating = 8, Date = new DateTime(2023, 10, 31), Recommended = true },

            // Monster Hunter World (game 20)
            new() { GameId = games[20].Id, ReviewerName = "HunterRank999", Title = "Addictive grind", Comment = "Easiest entry to the series. Hundreds of hours of monster slaying.", Rating = 9, Date = new DateTime(2024, 3, 7), Recommended = true },

            // Hades II (game 23)
            new() { GameId = games[23].Id, ReviewerName = "MelinoeMain", Title = "Even better than the first", Comment = "Supergiant has done it again. Bigger, deeper, and more beautiful.", Rating = 10, Date = new DateTime(2024, 6, 10), Recommended = true },
        };
        context.Reviews.AddRange(reviews);
        context.SaveChanges();
    }
}
