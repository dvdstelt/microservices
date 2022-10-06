namespace Red.Messages.ServiceComposerEvents;

public class AllMoviesLoaded
{
    public IDictionary<Guid, dynamic> AvailableMovies { get; set; }
}

