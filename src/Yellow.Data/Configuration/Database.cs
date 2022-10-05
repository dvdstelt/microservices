using Yellow.Data.Entities;

namespace Yellow.Data.Configuration;

public class Database
{
    static void Setup()
    {
        // Shared.Configuration.Database.Setup(DatabaseName, s =>
        // {
        //     var orders = s.GetCollection<Order>("orders");
        //
        //     orders.EnsureIndex(x => x.Identifier);
        //     orders.EnsureIndex(x => x.MovieIdentifier);
        // });
    }

    public const string DatabaseName = "Yellow";
}