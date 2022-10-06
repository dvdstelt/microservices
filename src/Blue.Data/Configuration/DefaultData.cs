using Blue.Data.Entities;

namespace Blue.Data.Configuration;

public class DefaultData
{
    static readonly Guid gotId = Guid.Parse("a9e9038d-c02f-410e-9f1d-c5a3c0a24fd4");
    static readonly Guid dune = Guid.Parse("1cc82d5e-6925-4c79-b5f0-f6264088312a");
    static readonly Guid maverick = Guid.Parse("17c8b4e4-ef16-4f52-ba5e-4f0a255af8f6");
    
    static readonly Guid rottterdam_kuip = Guid.Parse("eebe0187-f1eb-4971-a909-af3e77381a62");
    static readonly Guid rotterdam_centrum = Guid.Parse("bb5450d1-675c-478e-9945-fe789ad5e202");
    static readonly Guid denhaag = Guid.Parse("511b9a15-3dec-4ec8-afd2-32c1ac3bbb63");
    static readonly Guid scheveningen = Guid.Parse("65801707-17fc-4acd-b6a5-3f210f718fcc");
    static readonly Guid breda = Guid.Parse("308617eb-49c3-425f-82b6-97896683bd3b");
    static readonly Guid eindhoven = Guid.Parse("0e885038-5a2b-486f-b1fa-187bec2c9d96");       
    
    public static IEnumerable<Movie> GetDefaultMovies()
    {
        return new List<Movie>()
        {
            new Movie()
            {
                Identifier = Guid.Parse("a9e9038d-c02f-410e-9f1d-c5a3c0a24fd4"),
                UrlTitle = "gameofthrones",
                PopularityScore = 1000
            },
            new Movie()
            {
                Identifier = Guid.Parse("1cc82d5e-6925-4c79-b5f0-f6264088312a"),
                UrlTitle = "dune",
                PopularityScore = 500
            },
            new Movie()
            {
                Identifier = Guid.Parse("17c8b4e4-ef16-4f52-ba5e-4f0a255af8f6"),
                UrlTitle = "maverick",
                PopularityScore = 250
            }
        };
    }

    public static IEnumerable<Showing> GetDefaultShowings()
    {
        return new List<Showing>()
        {
            new Showing(dune, rottterdam_kuip, "10:00", 4),
            new Showing(dune, rottterdam_kuip, "13:00", 4),
            new Showing(dune, rotterdam_centrum, "10:00", 4),
            new Showing(dune, rotterdam_centrum, "13:00", 4),
            new Showing(dune, denhaag, "10:00", 4),
            new Showing(dune, denhaag, "13:00", 4),
            new Showing(maverick, rottterdam_kuip, "10:00", 4),
            new Showing(maverick, rottterdam_kuip, "13:00", 4),
            new Showing(maverick, rotterdam_centrum, "10:00", 4),
            new Showing(maverick, rotterdam_centrum, "13:00", 4),
            new Showing(maverick, denhaag, "10:00", 4),
            new Showing(maverick, denhaag, "13:00", 4),
        };
    }

    public static IEnumerable<Review> GetDefaultReviews()
    {
        return new List<Review>
        {
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = gotId,
                Description =
                    "A riddle: A father and his son fight in the Battle of the Bastards. The father is killed in battle and the son is brought to the Warden of the North to be knighted. The Warden of the North looks at the boy and says: \"I can't knight him, he's my son!\" How can this be?",
                ReviewedAt = new DateTime(2019, 05, 19, 23, 30, 12)
            },
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = gotId, Description = "I want more Arya! She's the best!",
                ReviewedAt = new DateTime(2019, 05, 19, 23, 35, 36)
            },
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = dune,
                Description = "Great visuals, screenplay adaptation is too close to the books though.",
                ReviewedAt = new DateTime(2021, 10, 8, 8, 14, 56)
            },
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = dune, Description = "Can't wait to go back to Mordor!",
                ReviewedAt = new DateTime(2021, 10, 9, 13, 37, 00)
            },
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = maverick,
                Description = "This movie needs to been in IMAX! That sound is incredible!",
                ReviewedAt = new DateTime(2022, 09, 21, 12, 12, 12)
            },
            new Review()
            {
                Identifier = Guid.NewGuid(), MovieIdentifier = maverick,
                Description = "Maverick is so much better than the original!",
                ReviewedAt = new DateTime(2022, 09, 20, 13, 45, 18)
            },
        };
    }
}