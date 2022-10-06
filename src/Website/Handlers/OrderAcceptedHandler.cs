using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Red.Data.Configuration;
using Red.Data.Entities;
using Red.Data.ViewModelComposition;
using Website.Hubs;
using Yellow.Messages.Events;

namespace Website.Handlers
{
    public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        readonly IHubContext<TicketHub> ticketHubContext;
        readonly ILogger<OrderAcceptedHandler> logger;
        readonly RedLiteDatabase db;

        public OrderAcceptedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderAcceptedHandler> logger, RedLiteDatabase db)
        {
            this.ticketHubContext = ticketHubContext;
            this.logger = logger;
            this.db = db;
        }

        public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                logger.LogError("Could not find SignalR ConnectionId from message headers");
                return;
            }
            
            //
            // *** TODO: This needs to be fixed! This violates the service boundary!
            //
            var movie = db.Query<Red.Data.Entities.Movie>().Where(s => s.Identifier == message.MovieId).Single();
            var theater = TheatersContext.GetTheaters().Single(s => s.Id == message.TheaterId);

            var ticket = new
            {
                message.OrderId,
                TheaterId = theater.Id.ToString(),
                Theater = theater.Name,
                MovieId = movie.Identifier.ToString(),
                MovieTitle = movie.Title,
                Time = message.Time,
                NumberOfTickets = message.NumberOfTickets
            };

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);
        }
    }
}
