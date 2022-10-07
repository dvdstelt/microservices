using Blue.Data.Configuration;
using Blue.Data.Entities;
using Blue.Messages.Events;
using NServiceBus;
using Yellow.Messages.Events;

namespace Blue.Ticketing.Handlers;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    readonly BlueLiteDatabase db;
    
    public OrderSubmittedHandler(BlueLiteDatabase db) => this.db = db;
    
    public async Task Handle(OrderSubmitted message, IMessageHandlerContext context)
    {
        var showing = db.Query<Showing>()
            .Where(v => v.MovieId == message.MovieId && v.TheaterId == message.TheaterId && v.Time == message.Time)
            .SingleOrDefault();

        if (showing == null)
        {
            await context.Publish(new SeatsApproved { OrderId = message.OrderId });
            return;
        }

        var allMatchingOrders = db.Query<Order>().Where(o =>
            o.MovieId == message.MovieId && o.TheaterId == message.TheaterId && o.Time == message.Time);
        var totalTicketsOrdered = allMatchingOrders.Select(s => s.NumberOfTickets).ToList().Sum();
        var totalTicketsLeft = showing.SeatsAvailable - totalTicketsOrdered;

        if (totalTicketsLeft < message.NumberOfTickets)
        {
            await context.Publish(new SeatsDenied() { OrderId = message.OrderId });
            return;
        }
        
        // All is good
        await context.Publish(new SeatsApproved { OrderId = message.OrderId });

        // Store this order
        var orderCollection = db.Database.GetCollection<Order>();
        orderCollection.Insert(new Order() { Identifier = message.OrderId ,MovieId = message.MovieId, TheaterId = message.TheaterId, Time = message.Time, NumberOfTickets = message.NumberOfTickets});
    }
}