using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Website.Hubs;
using Yellow.Messages.Events;

namespace Website.Handlers;

public class OrderDeniedHandler : IHandleMessages<OrderDenied>
{
    readonly IHubContext<TicketHub> ticketHubContext;
    readonly ILogger<OrderApprovedHandler> logger;

    public OrderDeniedHandler(IHubContext<TicketHub> ticketHubContext, ILogger<OrderApprovedHandler> logger)
    {
        this.ticketHubContext = ticketHubContext;
        this.logger = logger;
    }
    
    public async Task Handle(OrderDenied message, IMessageHandlerContext context)
    {
        logger.LogDebug("Oops, order denied.");
        
        if (!context.MessageHeaders.TryGetValue("SignalRConnectionId", out var userConnectionId))
        {
            logger.LogError("Could not find SignalR ConnectionId from message headers");
            return;
        }

        var ticket = new
        {
            success = false,
            message.OrderId,
        };
        
        await ticketHubContext.Clients.Client(userConnectionId).SendAsync("OrderedRegularTicket", ticket);
    }
}