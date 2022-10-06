﻿using System;
using NServiceBus;

namespace Shared.Configuration
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration, Action<RoutingSettings> configureRouting = null)
        {
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            var routing = transport.Routing();
            configureRouting?.Invoke(routing);

            endpointConfiguration.UsePersistence<LearningPersistence>();
            
            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new SignalR_Incoming(), "Stores SignalR user identifier into context.");
            pipeline.Register(new SignalR_Outgoing(), "Propagates SignalR user identifier to outgoing messages.");
            

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            // Don't do retries at all for this demo.
            endpointConfiguration.Recoverability().Immediate(s => s.NumberOfRetries(0));
            endpointConfiguration.Recoverability().Delayed(s => s.NumberOfRetries(0));

            return endpointConfiguration;
        }
    }
}
