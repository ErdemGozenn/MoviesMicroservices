using Movies.Core.Domain;
namespace Movies.APP.Domain;

public class MovieGenre:Record
{
    public int MovieId { get; set; }
    public int GenreId { get; set; }
    public Movie Movie { get; set; } = null!;
    public Genre Genre { get; set; } = null!;
}