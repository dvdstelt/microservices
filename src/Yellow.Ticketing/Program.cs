using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Configuration;
using Yellow.Ticketing.Behaviors;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("NServiceBus", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}) {Message}{NewLine}{Exception}",
        theme: SystemConsoleTheme.Literate)
    .CreateLogger();
    
var host = Host.CreateDefaultBuilder(args)
    .UseSerilog()
    .UseNServiceBus(context =>
    {
        var endpointConfiguration = new EndpointConfiguration("Yellow.Ticketing").ApplyCommonConfiguration();

        var pipeline = endpointConfiguration.Pipeline;
        pipeline.Register(new SignalR_Incoming(), "Stores SignalR user identifier into context.");
        pipeline.Register(new SignalR_Outgoing(), "Propagates SignalR user identifier to outgoing messages.");

        endpointConfiguration.RegisterComponents(s =>
        {
            s.ConfigureComponent(() => new LiteRepository(Database.DatabaseConnectionstring(Yellow.Data.Configuration.Database.DatabaseName)),
                DependencyLifecycle.InstancePerUnitOfWork);
        });
        
        return endpointConfiguration;
    })
    .Build();

var hostEnvironment = host.Services.GetRequiredService<IHostEnvironment>();
Console.Title = hostEnvironment.ApplicationName;

Log.Information("Press CTRL+C to quit the application...");

host.Run();

Log.Information("Application shut down...");
Log.CloseAndFlush();    