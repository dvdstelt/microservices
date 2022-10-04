using LiteDB;
using NServiceBus;
using NServiceBus.Logging;
using Yellow.Data.Entities;
using Yellow.Messages.Commands;
using Yellow.Messages.Events;

namespace Yellow.Ticketing.Handlers;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{
    static readonly ILog log = LogManager.GetLogger<SubmitOrderHandler>();
    
    private readonly LiteRepository db;
    
    public SubmitOrderHandler(LiteRepository db) => this.db = db;
    
    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        log.Info($"Order arrived for movie {message.MovieIdentifier}");

        context.Publish(new OrderAccepted() { OrderId = message.OrderIdentifier}).ConfigureAwait(false);        
        
        var order = new Order
        {
            Identifier = message.OrderIdentifier,
            MovieIdentifier = message.MovieIdentifier,
            TheaterIdentifier = message.TheaterIdentifier,
            UserIdentifier = message.UserId,
            TimeIdentifier = message.TimeIdentifier,
            NumberOfTickets = message.NumberOfTickets
        };
        
        db.Insert(order);
    }
}