using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Red.Data.Configuration;
using Red.Data.Entities;
using ServiceComposer.AspNetCore;

namespace Red.Data.ViewModelComposition;

public class MovieReviewsRequestHandler : ICompositionRequestsHandler
{
    readonly RedLiteDatabase db;

    public MovieReviewsRequestHandler(RedLiteDatabase db)
    {
        this.db = db;
    }
    
    [HttpGet("/reviews/{movieurl}")]
    public Task Handle(HttpRequest request)
    {
        var movieUrl = (string)request.HttpContext.GetRouteData().Values["movieurl"]!;
        var vm = request.GetComposedResponseModel();
        
        vm.Movie = db.Query<Movie>().Where(s => s.UrlTitle == movieUrl).Single();
        
        return Task.CompletedTask;
    }
}