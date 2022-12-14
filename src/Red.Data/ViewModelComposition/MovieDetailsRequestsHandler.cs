using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Red.Data.Configuration;
using Red.Data.Entities;
using ServiceComposer.AspNetCore;

namespace Red.Data.ViewModelComposition;

public class MovieDetailsRequestsHandler : ICompositionRequestsHandler
{
    readonly RedLiteDatabase db;

    public MovieDetailsRequestsHandler(RedLiteDatabase db)
    {
        this.db = db;
    }
    
    [HttpGet("/movies/{movieurl}")]
    public Task Handle(HttpRequest request)
    {
        var movieUrl = (string)request.HttpContext.GetRouteData().Values["movieurl"]!;
        var vm = request.GetComposedResponseModel();

        vm.Movie = db.Query<Movie>().Where(s => s.UrlTitle == movieUrl).Single();

        vm.Theaters = TheatersContext.GetTheaters();
        
        return Task.CompletedTask;        
    }
}