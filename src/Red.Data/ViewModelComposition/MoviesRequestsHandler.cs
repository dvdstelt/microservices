using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Red.Data.Configuration;
using Red.Data.Entities;
using ServiceComposer.AspNetCore;

namespace Red.Data.ViewModelComposition;

public class MoviesRequestsHandler : ICompositionRequestsHandler
{
    readonly RedLiteDatabase db;

    public MoviesRequestsHandler(RedLiteDatabase db)
    {
        this.db = db;
    }
    
    [HttpGet("/movies")]
    public Task Handle(HttpRequest request)
    {
        var vm = request.GetComposedResponseModel();
        
        var movies = db.Query<Movie>().ToList();

        vm.Movies = movies;

        return Task.CompletedTask;
    }
}