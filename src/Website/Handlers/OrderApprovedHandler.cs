using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Red.Data.Configuration;
using Red.Data.Entities;
using Website.Hubs;
using Yellow.Messages.Events;

namespace Website.Handlers
{
    public class OrderApprovedHandler : IHandleMessages<OrderApproved>
    {
        readonly IHubContext<TicketHub> ticketHubContext;
        readonly ILogger<OrderApprovedHandler> logger;
        readonly RedLiteDatabase db;

        public OrderApprovedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderApprovedHandler> logger, RedLiteDatabase db)
        {
            this.ticketHubContext = ticketHubContext;
            this.logger = logger;
            this.db = db;
        }

        public async Task Handle(OrderApproved message, IMessageHandlerContext context)
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
                success = true,
                message.OrderId,
                TheaterId = theater.Id.ToString(),
                Theater = theater.Name,
                MovieId = movie.Identifier.ToString(),
                MovieTitle = movie.Title,
                Time = message.Time,
                NumberOfTickets = message.NumberOfTickets
            };

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket, context.CancellationToken);
        }
    }
}
