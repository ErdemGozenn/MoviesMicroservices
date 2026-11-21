using Movies.Core.Domain;
using System.ComponentModel.DataAnnotations;
namespace Movies.APP.Domain;

public class Genre:Record
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public List<MovieGenre> MovieGenres { get; set; } = new();
}
