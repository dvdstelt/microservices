using Blue.Data.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Red.Data.Configuration;
using ServiceComposer.AspNetCore;
using Website.Hubs;
using Yellow.Data.Configuration;
using NServiceBus.Hosting.Helpers;
using AssemblyScanner = NServiceBus.Hosting.Helpers.AssemblyScanner;

namespace Website
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // We're using NServiceBus anyway, so let's use it to scan all assemblies.
            var assemblyScannerResults = new AssemblyScanner().GetScannableAssemblies();

            services.AddControllersWithViews();
            services.AddViewModelComposition(options =>
            {
                // options.AddMvcSupport();
                options.EnableCompositionOverControllers();
            });

            services.AddSignalR(o => o.EnableDetailedErrors = true);
            services.AddMediatR(assemblyScannerResults.Assemblies.ToArray());

            services.AddScoped<RedLiteDatabase>();
            services.AddScoped<BlueLiteDatabase>();
            services.AddScoped<YellowLiteDatabase>();
            services.AddScoped<MovieTickets>();

            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(builder =>
            {
                // ViewModelComposition
                builder.MapControllers();
                builder.MapCompositionHandlers();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<TicketHub>("/ticketHub");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");

                endpoints.MapControllerRoute(
                    name: "movie",
                    pattern: "{controller}/{movieurl}",
                    defaults: new { controller = "Movies", action = "Movie" });
            });
        }
    }
}