using NServiceBus;
using NServiceBus.Logging;
using Yellow.Data.Configuration;
using Yellow.Data.Entities;
using Yellow.Messages.Commands;

namespace Yellow.Ticketing.Handlers;

public class SubmitLotteryTicketHandler : IHandleMessages<SubmitLotteryTicket>
{
    static readonly ILog Log = LogManager.GetLogger<SubmitOrderHandler>();

    readonly YellowLiteDatabase db;
    
    public SubmitLotteryTicketHandler(YellowLiteDatabase db) => this.db = db;

    public Task Handle(SubmitLotteryTicket message, IMessageHandlerContext context)
    {
        Log.Info($"Order arrived for movie {message.MovieIdentifier}");

        var order = new LotteryTicket
        {
            Identifier = message.OrderIdentifier,
            MovieIdentifier = message.MovieIdentifier,
            TheaterIdentifier = message.TheaterIdentifier,
            UserIdentifier = message.UserId,
            NumberOfTickets = message.NumberOfTickets,
        };

        db.Insert(order);
        
        return Task.CompletedTask;
    }
}