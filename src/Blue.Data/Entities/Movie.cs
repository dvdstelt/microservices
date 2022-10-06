namespace Blue.Data.Entities;

public class Movie
{
    public Guid Identifier { get; set; }
    public string UrlTitle { get; set; }
    public int PopularityScore { get; set; }
}