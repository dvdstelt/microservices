using System.Security.Cryptography.X509Certificates;

namespace Yellow.Data.Entities;

public class Order
{
    public Guid Identifier { get; set; }
    public Guid UserIdentifier { get; set; }
    public Guid MovieIdentifier { get; set; }
    public Guid TheaterIdentifier { get; set; }
    public Guid TimeIdentifier { get; set; }
    public int NumberOfTickets { get; set; }
}