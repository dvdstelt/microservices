using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Website.Hubs
{
    public class MovieTickets
    {
        readonly IHubContext<TicketHub> hub;
        readonly ILogger<MovieTickets> logger;

        public MovieTickets(IHubContext<TicketHub> hub, ILogger<MovieTickets> logger)
        {
            this.hub = hub;
            this.logger = logger;
        }

        /// <summary>
        /// After ticket registration, immediately verify if it's a lottery ticket. 
        /// </summary>
        public async Task ReportOnLottery(MovieTicket ticket, string connectionId)
        {
            // var movieId = Guid.Parse(ticket.MovieId);
            // var theaterId = Guid.Parse(ticket.TheaterId);
            //
            // var movie = db.Query<Movie>()
            //     .Where(s => s.Id == movieId)
            //     .Single();
            //
            // if (movie.TicketType == TicketType.DrawingTicket)
            // {
            //     var theater = TheatersContext.GetTheaters().Single(s => s.Id == theaterId);                
            //     
            //     var message = new
            //     {
            //         Theater = theater.Name,
            //         MovieTitle = movie.Title,
            //         Time = ticket.Time,
            //         NumberOfTickets = ticket.NumberOfTickets,
            //         DrawingDate = DateTime.Now.AddDays(14).ToString("dddd, dd MMMM", CultureInfo.InvariantCulture)
            //     };                
            //
            //     logger.LogInformation("Sending OrderedLotteryTicket", message);                
            var message = new
            {
                MovieTitle = "Hello world"
            };
            await hub.Clients.Client(connectionId).SendAsync("OrderedLotteryTicket", message);
        }
    }
}