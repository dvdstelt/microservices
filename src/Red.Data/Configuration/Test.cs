using LiteDB;
using Red.Data.Entities;

namespace Red.Data.Configuration;

public class Test : LiteRepository
{
    const string DatabaseName = "red.db";

    public Test() : this(Shared.Configuration.Database.DatabaseConnectionstring(DatabaseName))
    {
        var movieCollection = this.Database.GetCollection<Movie>();
        var reviewCollection = this.Database.GetCollection<Review>();

        if (movieCollection.Count() == 0)
        {
            var movies = DefaultData.GetDefaultMovies().ToList();
            var reviews = DefaultData.GetDefaultReviews(movies);
                
            movieCollection.Insert(movies);
            reviewCollection.Insert(reviews);

            movieCollection.EnsureIndex(x => x.Identifier);
            movieCollection.EnsureIndex(x => x.Title);
            reviewCollection.EnsureIndex(x => x.Identifier);
            reviewCollection.EnsureIndex(x => x.MovieIdentifier);
        }
    }
    
    protected Test(ILiteDatabase database) : base(database)
    {
    }

    protected Test(string connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
    {
    }

    protected Test(ConnectionString connectionString, BsonMapper mapper = null) : base(connectionString, mapper)
    {
    }

    protected Test(Stream stream, BsonMapper mapper = null, Stream logStream = null) : base(stream, mapper, logStream)
    {
    }
}