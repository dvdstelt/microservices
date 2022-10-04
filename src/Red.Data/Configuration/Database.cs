using Red.Data.Entities;

namespace Red.Data.Configuration;

public class Database
{
    static void Setup()
    {
        Shared.Configuration.Database.Setup(DatabaseName, s =>
        {
            var movieCollection = s.GetCollection<Movie>("movies");
            var reviewCollection = s.GetCollection<Review>("reviews");

            if (movieCollection.Count() == 0)
            {
                var movies = DefaultData.GetDefaultMovies();
                var reviews = DefaultData.GetDefaultReviews();
                
                movieCollection.Insert(movies);
                reviewCollection.Insert(reviews);

                movieCollection.EnsureIndex(x => x.Identifier);
                movieCollection.EnsureIndex(x => x.Title);
                reviewCollection.EnsureIndex(x => x.Identifier);
                reviewCollection.EnsureIndex(x => x.MovieIdentifier);
            }

        });
    }

   const string DatabaseName = "Red";
}