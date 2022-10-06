using Microsoft.AspNetCore.Mvc.Diagnostics;
using Red.Data.Entities;

namespace Red.Data.Configuration
{
    static class DefaultData
    {
        public static IEnumerable<Movie> GetDefaultMovies()
        {
            return new List<Movie>()
            {
                new Movie
                {
                    Identifier = Guid.Parse("a9e9038d-c02f-410e-9f1d-c5a3c0a24fd4"),
                    UrlTitle = "gameofthrones",
                    Title = "Game of Thrones",
                    Image = "gameofthrones.jpg",
                    Rating = 5,
                    Description =
                        "Watch this last episode on the big screen! In the aftermath of the devastating attack on King&#39;s Landing, Daenerys must face the survivors.",
                    Icons = new List<string> {"16", "sex", "violence"},
                    MovieDetails = "80 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2019, 05, 19),
                    Showtimes = new List<string> {"19:00"},
                    PricePerTicket = 0D,
                    TicketType = TicketType.DrawingTicket,
                },
                //new Movie
                //{
                //    Id = Guid.NewGuid(),
                //    UrlTitle = "jayandsilentbobreboot",
                //    Title = "Jay and Silent Bob Reboot",
                //    Image = "jayandsilentbobreboot.jpg",
                //    Rating = 2,
                //    Description =
                //        "Jay and Silent Bob return to Hollywood to stop a reboot of &#39;Bluntman and Chronic&#39; movie from getting made.",
                //    Icons = new List<string> {"16", "alcoholdrugabuse", "explicitlanguage"},
                //    MovieDetails = "105 minutes | English, Dutch subtitles",
                //    ReleaseDate = new DateTime(2019, 10, 15),
                //    Showtimes = new List<string> {"10:00", "15:00", "20:00"},
                //    PricePerTicket = 10D,
                //    PopularityScore = 200,
                //},
                new Movie
                {
                    Identifier = Guid.Parse("1cc82d5e-6925-4c79-b5f0-f6264088312a"),
                    UrlTitle = "dune",
                    Title = "Dune",
                    Image = "dune.jpg",
                    Rating = 5,
                    Description =
                        "Feature adaptation of Frank Herbert's science fiction novel, about the son of a noble family entrusted with the protection of the most valuable asset and most vital element in the galaxy.",
                    Icons = new List<string> {"12", "violence", "fear" },
                    MovieDetails = "156 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2021, 9, 16),
                    Showtimes = new List<string> {"10:00", "13:00", "15:00", "20:00", "23:00"},
                    PricePerTicket = 20D,
                },
                // new Movie
                // {
                //     Id = Guid.NewGuid(),
                //     UrlTitle = "freeguy",
                //     Title = "FreeGuy",
                //     Image = "freeguy.jpg",
                //     Rating = 5,
                //     Description = "A bank teller discovers that he's actually an NPC inside a brutal, open world video game.",
                //     Icons = new List<string> {"12", "violence", "explicitlanguage" },
                //     MovieDetails = "115 minutes | English, Dutch subtitles",
                //     ReleaseDate = new DateTime(2021,08,11),
                //     Showtimes = new List<string> { "15:00", "20:00", "23:00" },
                //     PricePerTicket = 10D,
                //     PopularityScore = 250
                // }
                new Movie
                {
                    Identifier = Guid.Parse("17c8b4e4-ef16-4f52-ba5e-4f0a255af8f6"),
                    UrlTitle = "maverick",
                    Title = "Top Gun Maverick",
                    Image = "maverick.jpg",
                    Rating = 5,
                    Description = "After thirty years, Maverick is still pushing the envelope as a top naval aviator, but must confront ghosts of his past when he leads TOP GUN's elite graduates on a mission that demands the ultimate sacrifice from those chosen to fly it.",
                    Icons = new List<string> {"12", "violence", "explicitlanguage" },
                    MovieDetails = "2 hours 10 minutes | English, Dutch subtitles",
                    ReleaseDate = new DateTime(2022,04,27),
                    Showtimes = new List<string> { "15:00", "20:00", "23:00" },
                    PricePerTicket = 10D,
                }
            };
        }
    }
}
