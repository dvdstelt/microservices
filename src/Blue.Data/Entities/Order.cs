namespace Blue.Data.Entities;

public class Order
{
    public Guid Identifier { get; set; }
    public Guid MovieId { get; set; }
    public Guid TheaterId { get; set; }
    public string Time { get; set; }
    public int NumberOfTickets { get; set; }
}