using Movies.Core.Domain;
using System.ComponentModel.DataAnnotations;
namespace Movies.APP.Domain;

public class Director:Record
{
    [Required]
    public string FirstName { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    public bool IsRetired { get; set; }
    public List<Movie> Movies { get; set; } = new();
}
