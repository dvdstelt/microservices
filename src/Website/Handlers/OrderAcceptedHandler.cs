using System.Linq;
using System.Threading.Tasks;
using EventualConsistencyDemo.Hubs;
using LiteDB;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Yellow.Messages.Events;

namespace Website.Handlers
{
    public class OrderAcceptedHandler : IHandleMessages<OrderAccepted>
    {
        readonly IHubContext<TicketHub> ticketHubContext;
        private readonly LiteRepository db;
        readonly ILogger<OrderAcceptedHandler> logger;

        public OrderAcceptedHandler(IHubContext<TicketHub> ticketHubContext, LiteRepository db, ILogger<OrderAcceptedHandler> logger)
        {
            this.ticketHubContext = ticketHubContext;
            this.db = db;
            this.logger = logger;
        }

        public async Task Handle(OrderAccepted message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                logger.LogError("Could not find SignalR ConnectionId from message headers");
                return;
            }
            
            // TODO: Nu hier reference maken naar iets van Red.Data en dan data queryen?
            
            // var movie = db.Query<Movie>().Where(s => s.Id == message.Movie).SingleOrDefault();
            // var theater = TheatersContext.GetTheaters().Single(s => s.Id == message.Theater);
            //
            // if (movie.TicketType == TicketType.DrawingTicket)
            //     return;

            var ticket = new
            {
                message.OrderId
                // TheaterId = theater.Id.ToString(),
                // Theater = theater.Name,
                // MovieId = movie.Id.ToString(),
                // MovieTitle = movie.Title,
                // Time = message.MovieTime,
                // NumberOfTickets = message.NumberOfTickets
            };

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);
        }
    }
}
