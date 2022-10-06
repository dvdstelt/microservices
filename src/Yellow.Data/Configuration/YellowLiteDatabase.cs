using LiteDB;
using Yellow.Data.Entities;

namespace Yellow.Data.Configuration;

public class YellowLiteDatabase : LiteRepository
{
    const string DatabaseName = "yellow.db";

    public YellowLiteDatabase() : this(Shared.Configuration.Database.DatabaseConnectionstring(DatabaseName))
    {
        var orderCollection = this.Database.GetCollection<Order>();
        
        orderCollection.EnsureIndex(x => x.Identifier);
        orderCollection.EnsureIndex(x => x.MovieIdentifier);
    }

    protected YellowLiteDatabase(ILiteDatabase database) : base(database)
    {
    }

    protected YellowLiteDatabase(string connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected YellowLiteDatabase(ConnectionString connectionString, BsonMapper? mapper = null) : base(connectionString, mapper)
    {
    }

    protected YellowLiteDatabase(Stream stream, BsonMapper? mapper = null, Stream? logStream = null) : base(stream, mapper, logStream)
    {
    }
}