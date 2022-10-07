using Blue.Data.Configuration;
using Blue.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ServiceComposer.AspNetCore;

namespace Blue.Data.ViewModelComposition;

public class MovieReviewsRequestsHandler : ICompositionRequestsHandler
{
    readonly BlueLiteDatabase db;

    public MovieReviewsRequestsHandler(BlueLiteDatabase db) => this.db = db;
    
    [HttpGet("/reviews/{movieurl}")]
    public Task Handle(HttpRequest request)
    {
        var movieUrl = (string)request.HttpContext.GetRouteData().Values["movieurl"]!;
        var vm = request.GetComposedResponseModel();
        
        var movie = db.Query<Movie>().Where(s => s.UrlTitle == movieUrl).Single();
        var reviews = db.Query<Review>().Where(s => s.MovieIdentifier == movie.Identifier).ToList();

        vm.Reviews = reviews;
        
        return Task.CompletedTask;
    }
}