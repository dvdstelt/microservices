namespace Blue.Data.Entities;

public class Showing
{
    public Showing(Guid movieId, Guid theaterId, string time, int seatsAvailable)
    {
        Identifier = Guid.NewGuid();
        MovieId = movieId;
        TheaterId = theaterId;
        Time = time;
        SeatsAvailable = seatsAvailable;
    }

    public Guid Identifier { get; set; }
    public Guid MovieId { get; set; }
    public Guid TheaterId { get; set; }
    public string Time { get; set; }
    public int SeatsAvailable { get; set; }
}