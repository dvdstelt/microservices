using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared.Configuration;
using Website;
using Yellow.Messages.Commands;


var host = Host.CreateDefaultBuilder(args)
    // NServiceBus needs to be configured first!
    .UseNServiceBus(hostBuilderContext =>
    {
        var endpointConfiguration = new EndpointConfiguration("EventualConsistencyDemo");
        endpointConfiguration.ApplyCommonConfiguration(routingConfig =>
        {
            routingConfig.RouteToEndpoint(typeof(SubmitOrder), "server");
        });

        return endpointConfiguration;
    })
    .ConfigureWebHostDefaults(c => c.UseStartup<Startup>())
    .Build();

// Create the LiteDb database so we can work with some default movies.
// TODO: This doesn't work anymore now we have multiple services. Figure out if still required.
// Database.Setup();

await host.RunAsync();