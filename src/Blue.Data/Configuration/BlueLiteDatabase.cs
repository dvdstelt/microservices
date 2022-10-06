using Blue.Data.Entities;
using LiteDB;

namespace Blue.Data.Configuration;

public class BlueLiteDatabase : LiteRepository
{
    const string DatabaseName = "blue.db";
    
    public BlueLiteDatabase() : this(Shared.Configuration.Database.DatabaseConnectionstring(DatabaseName))
    {
        var movieCollection = this.Database.GetCollection<Movie>();
        var reviewCollection = this.Database.GetCollection<Review>();

        if (movieCollection.Count() == 0)
        {
            var movies = DefaultData.GetDefaultMovies().ToList();
            var reviews = DefaultData.GetDefaultReviews().ToList();
            
            movieCollection.Insert(movies);
            reviewCollection.Insert(reviews);
            
            movieCollection.EnsureIndex(x => x.Identifier);
            reviewCollection.EnsureIndex(x => x.Identifier);
            reviewCollection.EnsureIndex(x => x.MovieIdentifier);
        }
    }
    
    protected BlueLiteDatabase(ILiteDatabase database) : base(database)
    {
    }

    protected BlueLiteDatabase(string connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected BlueLiteDatabase(ConnectionString connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected BlueLiteDatabase(Stream stream, BsonMapper? mapper = null, Stream? logStream = null) : base(stream, mapper, logStream)
    {
    }
}