namespace Yellow.Messages.Commands;

public class SubmitLotteryTicket
{
    public Guid OrderIdentifier { get; set; }
    public Guid TheaterIdentifier { get; set; }
    public Guid MovieIdentifier { get; set; }
    public int NumberOfTickets { get; set; }
    public Guid UserId { get; set; }
}