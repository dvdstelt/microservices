namespace Yellow.Data.Entities;

public class LotteryTicket
{
    public Guid Identifier { get; set; }
    public Guid UserIdentifier { get; set; }
    public Guid MovieIdentifier { get; set; }
    public Guid TheaterIdentifier { get; set; }
    public int NumberOfTickets { get; set; }
}