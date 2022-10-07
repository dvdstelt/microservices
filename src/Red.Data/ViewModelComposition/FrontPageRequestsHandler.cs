using System.Dynamic;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Red.Data.Configuration;
using Red.Data.Entities;
using Red.Messages.ServiceComposerEvents;
using ServiceComposer.AspNetCore;

namespace Red.Data.ViewModelComposition;

public class FrontPageRequestsHandler : ICompositionRequestsHandler
{
    readonly LiteRepository db;

    public FrontPageRequestsHandler(RedLiteDatabase db) => this.db = db;

    [HttpGet("/")]
    public async Task Handle(HttpRequest request)
    {
        var vm = request.GetComposedResponseModel();
        
        var movies = db.Query<Movie>().ToList();
        var availableMovies = MapToDictionary(movies);

        await vm.RaiseEvent(new AllMoviesLoaded() { AvailableMovies = availableMovies });

        vm.Movies = availableMovies.Values.ToList();
    }

    IDictionary<Guid, dynamic> MapToDictionary(IEnumerable<Movie> movies)
    {
        var allMovies = new Dictionary<Guid, dynamic>();

        foreach (var movie in movies)
        {
            dynamic vm = new ExpandoObject();
            vm.Identifier = movie.Identifier;
            vm.UrlTitle = movie.UrlTitle;
            vm.Title = movie.Title;
            vm.Description = movie.Description;
            vm.Image = movie.Image;
            vm.Rating = movie.Rating;
            vm.Icons = movie.Icons;
            vm.MovieDetails = movie.MovieDetails;
            vm.ReleaseDate = movie.ReleaseDate;
            vm.PricePerTicket = movie.PricePerTicket;
            vm.TicketType = movie.TicketType;

            allMovies[movie.Identifier] = vm;
        }
        
        return allMovies;
    }
}