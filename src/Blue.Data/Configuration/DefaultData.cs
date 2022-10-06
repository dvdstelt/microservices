using Blue.Data.Entities;

namespace Blue.Data.Configuration;

public class DefaultData
{
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

    public static IEnumerable<Review> GetDefaultReviews()
    {
        var gotId = Guid.Parse("a9e9038d-c02f-410e-9f1d-c5a3c0a24fd4");
        var dune = Guid.Parse("1cc82d5e-6925-4c79-b5f0-f6264088312a");
        var maverick = Guid.Parse("17c8b4e4-ef16-4f52-ba5e-4f0a255af8f6");

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
            //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Kevin Smith is my favorite director and this movie is another great piece of work.", ReviewedAt = new DateTime(2019, 10, 8, 12, 12, 12) },
            //new Review() { Id = Guid.NewGuid(), MovieIdentifier = jsbId, Description = "Snootch to the nootch!!!", ReviewedAt = new DateTime(2019, 10, 9, 13, 45, 18) },
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