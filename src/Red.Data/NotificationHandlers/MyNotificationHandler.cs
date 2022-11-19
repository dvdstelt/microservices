using MediatR;
using Red.Data.Configuration;
using Red.Data.Entities;
using Shared.Notifications;

namespace Red.Data.NotificationHandlers;

public class MyNotificationHandler : INotificationHandler<MyNotification>
{
    private readonly RedLiteDatabase db;

    public MyNotificationHandler(RedLiteDatabase db)
    {
        this.db = db;
    }
    
    public Task Handle(MyNotification notification, CancellationToken cancellationToken)
    {
        var movieId = notification.MovieId;
        var movie = db.Query<Red.Data.Entities.Movie>().Where(s => s.Identifier == movieId).Single();
        notification.MovieTitle = movie.Title;
        
        var theater = TheatersContext.GetTheaters().Single(s => s.Id == notification.TheaterId);
        notification.TheaterName = theater.Name;

        return Task.CompletedTask;
    }
}