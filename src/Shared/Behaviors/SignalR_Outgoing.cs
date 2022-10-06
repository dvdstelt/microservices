using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace Shared
{
    public class SignalR_Outgoing : Behavior<IOutgoingLogicalMessageContext>
    {
        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            if (context.Extensions.TryGet("SignalRConnectionId", out string signalRConnectionId))
            {
                context.Headers["SignalRConnectionId"] = signalRConnectionId;
            }
            return next();
        }
    }
}