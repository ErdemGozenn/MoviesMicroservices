using Movies.Core.Domain;
using System.ComponentModel.DataAnnotations;
namespace Movies.APP.Domain;

public class Movie:Record
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int DirectorId { get; set; }
    public Director Director { get; set; } = null!;
    public List<MovieGenre> MovieGenres { get; set; } = new();
}
