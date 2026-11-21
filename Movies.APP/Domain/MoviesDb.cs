using Microsoft.EntityFrameworkCore;
namespace Movies.APP.Domain;

public class MoviesDb:DbContext
{
    public DbSet<MovieGenre> MovieGenres { get; set; } = null!;
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Director> Directors { get; set; } = null!;
    
    public MoviesDb(DbContextOptions<MoviesDb> options) : base(options) { }
}