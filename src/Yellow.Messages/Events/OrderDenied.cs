namespace Yellow.Messages.Events;

public class OrderDenied
{
    public OrderDenied(Guid orderId, string message)
    {
        OrderId = orderId;
        Message = message;
    }
    
    public Guid OrderId { get; set; }
    public string Message { get; set; }
}