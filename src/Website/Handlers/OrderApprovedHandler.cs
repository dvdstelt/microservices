using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Red.Data.Configuration;
using Red.Data.Entities;
using Shared.Notifications;
using Website.Hubs;
using Yellow.Messages.Events;

namespace Website.Handlers
{
    public class OrderApprovedHandler : IHandleMessages<OrderApproved>
    {
        readonly IHubContext<TicketHub> ticketHubContext;
        readonly ILogger<OrderApprovedHandler> logger;
        private readonly IMediator mediator;

        public OrderApprovedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderApprovedHandler> logger, IMediator mediator)
        {
            this.ticketHubContext = ticketHubContext;
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task Handle(OrderApproved message, IMessageHandlerContext context)
        {
            if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
            {
                logger.LogError("Could not find SignalR ConnectionId from message headers");
                return;
            }

            var notification = new MyNotification();
            notification.MovieId = message.MovieId;
            notification.TheaterId = message.TheaterId;

            await mediator.Publish(notification, context.CancellationToken);
            
            var ticket = new
            {
                success = true,
                message.OrderId,
                theater = notification.TheaterName,
                movieTitle = notification.MovieTitle,
                Time = message.Time,
                NumberOfTickets = message.NumberOfTickets
            };

            await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket, context.CancellationToken);
        }
    }
}
