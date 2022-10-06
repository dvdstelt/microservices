using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Red.Data.Configuration;

namespace Website.Hubs
{
    public class MovieTickets
    {
        readonly IHubContext<TicketHub> hub;
        readonly ILogger<MovieTickets> logger;
        readonly RedLiteDatabase db;

        public MovieTickets(IHubContext<TicketHub> hub, ILogger<MovieTickets> logger, RedLiteDatabase db)
        {
            this.hub = hub;
            this.logger = logger;
            this.db = db;
        }

        /// <summary>
        /// After ticket registration, immediately verify if it's a lottery ticket. 
        /// </summary>
        public async Task ReportOnLottery(MovieTicket ticket, string connectionId)
        {
            var movieId = Guid.Parse(ticket.MovieId);
            var theaterId = Guid.Parse(ticket.TheaterId);
            
            var movie = db.Query<Red.Data.Entities.Movie>()
                .Where(s => s.Identifier == movieId)
                .Single();

            if (movie.TicketType == Red.Data.Entities.TicketType.DrawingTicket)
            {
                var theater = Red.Data.Entities.TheatersContext.GetTheaters().Single(s => s.Id == theaterId);

                var message = new
                {
                    Theater = theater.Name,
                    MovieTitle = movie.Title,
                    Time = ticket.Time,
                    NumberOfTickets = ticket.NumberOfTickets,
                    DrawingDate = DateTime.Now.AddDays(14).ToString("dddd, dd MMMM", CultureInfo.InvariantCulture)
                };

                logger.LogInformation("Sending OrderedLotteryTicket", message);

                await hub.Clients.Client(connectionId).SendAsync("OrderedLotteryTicket", message);
            }
        }
    }
}