using System;
using NServiceBus;

namespace Shared.Configuration
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration ApplyCommonConfiguration(this EndpointConfiguration endpointConfiguration,
            Action<RoutingSettings> configureRouting = null)
        {
            var transport = endpointConfiguration.UseTransport(new LearningTransport());
            configureRouting?.Invoke(transport);

            endpointConfiguration.UsePersistence<LearningPersistence>();

            var pipeline = endpointConfiguration.Pipeline;
            pipeline.Register(new SignalR_Incoming(), "Stores SignalR user identifier into context.");
            pipeline.Register(new SignalR_Outgoing(), "Propagates SignalR user identifier to outgoing messages.");

            ConfigurePlatform(endpointConfiguration);

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            // Don't do retries at all for this demo.
            endpointConfiguration.Recoverability().Immediate(s => s.NumberOfRetries(0));
            endpointConfiguration.Recoverability().Delayed(s => s.NumberOfRetries(0));

            return endpointConfiguration;
        }

        static void ConfigurePlatform(EndpointConfiguration endpointConfiguration)
        {
            var servicePlatformConnection = ServicePlatformConnectionConfiguration.Parse(@"{
                    ""Heartbeats"": {
                        ""Enabled"": false,
                        ""HeartbeatsQueue"": ""Particular.ServiceControl"",
                        ""Frequency"": ""00:00:10"",
                        ""TimeToLive"": ""00:00:40""
                    },
                    ""MessageAudit"": {
                        ""Enabled"": true,
                        ""AuditQueue"": ""audit""
                    },
                    ""CustomChecks"": {
                        ""Enabled"": false,
                        ""CustomChecksQueue"": ""Particular.ServiceControl""
                    },
                    ""ErrorQueue"": ""error"",
                    ""SagaAudit"": {
                        ""Enabled"": true,
                        ""SagaAuditQueue"": ""audit""
                    },
                    ""Metrics"": {
                        ""Enabled"": true,
                        ""MetricsQueue"": ""Particular.Monitoring"",
                        ""Interval"": ""00:00:01""
                    }
                }");

            endpointConfiguration.ConnectToServicePlatform(servicePlatformConnection);
        }
    }
}