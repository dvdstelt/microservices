using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NServiceBus;
using Red.Data.Configuration;
using Red.Data.Entities;
using Yellow.Messages.Commands;

namespace Website.Hubs
{
    public class TicketHub : Hub
    {
        readonly MovieTickets movieTickets;
        readonly RedLiteDatabase db;

        public TicketHub(MovieTickets movieTickets, RedLiteDatabase db)
        {
            this.movieTickets = movieTickets;
            this.db = db;
        }

        public async Task SubmitOrder(MovieTicket ticket)
        {
            // Connection for this visitor
            var userConnectionId = this.Context.ConnectionId;
           
            // Get movie from database
            var movieId = Guid.Parse(ticket.MovieId);            
            var movie = db.Query<Red.Data.Entities.Movie>()
                .Where(s => s.Identifier == movieId)
                .Single();

            #region Deal with lottery ticket
            if (movie.TicketType == TicketType.DrawingTicket)
            {
                var message = new SubmitLotteryTicket
                {
                    OrderIdentifier = Guid.NewGuid(),
                    TheaterIdentifier = Guid.Parse(ticket.TheaterId),
                    MovieIdentifier = Guid.Parse(ticket.MovieId),
                    NumberOfTickets = ticket.NumberOfTickets,
                    UserId = Guid.Parse("218d92c4-9c42-4e61-80fa-198b22461f61"), // For now, no other users allowed ;-)
                };

                await messageSession.Send(message);
                
                await movieTickets.ReportOnLottery(ticket, Context.ConnectionId);
                return;
            }
            #endregion
        }
    }
}