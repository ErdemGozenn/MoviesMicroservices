using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Movies;

public class MovieAddRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }

    [Range(1, int.MaxValue)]
    public int DirectorId { get; set; }

    public List<int> GenreIds { get; set; } = new();
}

public class MovieAddHandler : DbService<Movie>,
    IRequestHandler<MovieAddRequest, CommandResponse>
{
    private readonly MoviesDb _moviesDb;

    public MovieAddHandler(MoviesDb db) : base(db)
    {
        _moviesDb = db;
    }

    public async Task<CommandResponse> Handle(
        MovieAddRequest request,
        CancellationToken cancellationToken)
    {
        var genreIds = (request.GenreIds ?? new List<int>())
            .Distinct()
            .ToList();

        if (!await _moviesDb.Directors.AnyAsync(
                director => director.Id == request.DirectorId,
                cancellationToken))
        {
            return Error("Director not found!");
        }

        var existingGenreCount = await _moviesDb.Genres
            .CountAsync(genre => genreIds.Contains(genre.Id), cancellationToken);

        if (existingGenreCount != genreIds.Count)
        {
            return Error("One or more genres were not found!");
        }
        var movie = new Movie
        {
            Name = request.Name.Trim(),
            ReleaseDate = request.ReleaseDate,
            TotalRevenue = request.TotalRevenue,
            DirectorId = request.DirectorId,
            MovieGenres = genreIds
                .Select(genreId => new MovieGenre { GenreId = genreId })
                .ToList()
        };

        await DbAddAsync(movie, cancellationToken);

        return Success(movie.Id, "Movie created successfully.");
    }
}