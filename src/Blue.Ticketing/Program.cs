using Blue.Data.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Shared.Configuration;

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
        var endpointConfiguration = new EndpointConfiguration("Blue.Ticketing").ApplyCommonConfiguration();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        
        endpointConfiguration.RegisterComponents(s =>
        {
            s.ConfigureComponent(() => new BlueLiteDatabase(), DependencyLifecycle.InstancePerUnitOfWork);
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