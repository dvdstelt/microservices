using NServiceBus;
using Yellow.Messages.Events;

namespace Yellow.Ticketing.Handlers;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        // arrange payment
        await context.Publish(new OrderPaid() { OrderId = message.OrderId });
    }
}