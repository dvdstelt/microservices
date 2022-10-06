using System.Net.Mail;
using NServiceBus;
using NServiceBus.Logging;
using Red.Data.Configuration;
using Red.Data.Entities;
using Yellow.Messages.Events;

namespace Yellow.Ticketing.Handlers;

public class OrderApprovedHandler : IHandleMessages<OrderApproved>
{
    static readonly ILog Log = LogManager.GetLogger<OrderApprovedHandler>();

    readonly RedLiteDatabase db;
    
    public OrderApprovedHandler(RedLiteDatabase db) => this.db = db;
    
    public Task Handle(OrderApproved message, IMessageHandlerContext context)
    {
        var movie = db.Query<Movie>().Where(m => m.Identifier == message.MovieId).Single();
        var theater = TheatersContext.GetTheaters().Single(t => t.Id == message.TheaterId);

        var body =
            $"Hi,\n\nYou ordered {message.NumberOfTickets} tickets for the movie '{movie.Title}' in theater {theater.Name} at {message.Time}.\n\nWe hope you enjoy your time at our theater.";
        
        var smtpClient = new SmtpClient("localhost")
        {
            Port = 25,
            // Credentials = new NetworkCredential("username", "password"),
            // EnableSsl = true,
        };
    
        smtpClient.Send("info@compilesoftware.nl", "dennis@bloggingabout.net", $"Your ticket for {movie.Title}", body);
        
        return Task.CompletedTask;
    }
}