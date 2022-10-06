using LiteDB;
using Red.Data.Entities;

namespace Red.Data.Configuration;

public class RedLiteDatabase : LiteRepository
{
    const string DatabaseName = "red.db";

    public RedLiteDatabase() : this(Shared.Configuration.Database.DatabaseConnectionstring(DatabaseName))
    {
        var movieCollection = this.Database.GetCollection<Movie>();

        if (movieCollection.Count() == 0)
        {
            var movies = DefaultData.GetDefaultMovies().ToList();
                
            movieCollection.Insert(movies);

            movieCollection.EnsureIndex(x => x.Identifier);
            movieCollection.EnsureIndex(x => x.Title);
        }
    }
    
    protected RedLiteDatabase(ILiteDatabase database) : base(database)
    {
    }

    protected RedLiteDatabase(string connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected RedLiteDatabase(ConnectionString connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected RedLiteDatabase(Stream stream, BsonMapper? mapper = null, Stream? logStream = null) : base(stream, mapper, logStream)
    {
    }
}