using Blue.Data.Configuration;
using Blue.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Red.Messages.ServiceComposerEvents;
using ServiceComposer.AspNetCore;

namespace Blue.Data.ViewModelComposition;

public class MovieDetailsSubscriber : ICompositionEventsSubscriber
{
    readonly BlueLiteDatabase db;

    public MovieDetailsSubscriber(BlueLiteDatabase db) => this.db = db;
    
    [HttpGet("/")]
    public void Subscribe(ICompositionEventsPublisher publisher)
    {
        publisher.Subscribe<AllMoviesLoaded>((@event, request) =>
        {
            var moviesPopularity = db.Query<Movie>().ToList();

            foreach (var availableMovie in @event.AvailableMovies)
            {
                var movie = moviesPopularity.Single(s => s.Identifier == availableMovie.Key);
                availableMovie.Value.PopularityScore = movie.PopularityScore;
            }

            return Task.CompletedTask;
        });
    }
}