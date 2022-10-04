namespace Yellow.Messages.Commands
{
    public class SubmitOrder
    {
        public Guid OrderIdentifier { get; set; }
        public Guid UserId { get; set; }
        public Guid MovieIdentifier { get; set; }
        public Guid TheaterIdentifier { get; set; }
        public Guid TimeIdentifier { get; set; }
        public int NumberOfTickets { get; set; }
    }
}
