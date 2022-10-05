using System.Runtime.Intrinsics.Arm;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NServiceBus.Logging;
using Red.Data.Configuration;
using Red.Data.Entities;
using ServiceComposer.AspNetCore;
using Shared.Configuration;

namespace Red.Data.ViewModelComposition;

public class AllMoviesRequestsHandler : ICompositionRequestsHandler
{
    readonly LiteRepository db;
    static readonly ILog log = LogManager.GetLogger<AllMoviesRequestsHandler>();

    public AllMoviesRequestsHandler(Test db)
    {
        this.db = db;
    }

    [HttpGet("/")]
    public Task Handle(HttpRequest request)
    {
        var vm = request.GetComposedResponseModel();
        
        vm.Movies = db.Query<Movie>().ToList();

        return Task.CompletedTask;
    }
    
    // [HttpGet("/movies/{id}")]
    // public Task Handle(HttpRequest request)
    // {
    //     var vm = request.GetComposedResponseModel();
    //     
    //     vm.Movies = db.Query<Movie>().ToList();
    //
    //     return Task.CompletedTask;
    // }
}