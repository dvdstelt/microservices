using LiteDB;
using NServiceBus;
using NServiceBus.Logging;
using Yellow.Data.Configuration;
using Yellow.Data.Entities;
using Yellow.Messages.Commands;
using Yellow.Messages.Events;

namespace Yellow.Ticketing.Handlers;

public class SubmitOrderHandler : IHandleMessages<SubmitOrder>
{
    static readonly ILog Log = LogManager.GetLogger<SubmitOrderHandler>();

    private readonly YellowLiteDatabase db;

    public SubmitOrderHandler(YellowLiteDatabase db) => this.db = db;

    public Task Handle(SubmitOrder message, IMessageHandlerContext context)
    {
        Log.Info($"Order arrived for movie {message.MovieIdentifier}");

        context.Publish(new OrderAccepted()
        {
            OrderId = message.OrderIdentifier, 
            MovieId = message.MovieIdentifier, 
            TheaterId = message.TheaterIdentifier,
            Time = message.Time,
            NumberOfTickets = message.NumberOfTickets
        }).ConfigureAwait(false);

        var order = new Order
        {
            Identifier = message.OrderIdentifier,
            MovieIdentifier = message.MovieIdentifier,
            TheaterIdentifier = message.TheaterIdentifier,
            UserIdentifier = message.UserId,
            NumberOfTickets = message.NumberOfTickets,
            MovieTime = message.Time
        };

        db.Insert(order);

        return Task.CompletedTask;
    }
}