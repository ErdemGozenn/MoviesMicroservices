using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Movies;

public class MovieUpdateRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }

    [Range(1, int.MaxValue)]
    public int DirectorId { get; set; }

    public List<int> GenreIds { get; set; } = new();
}

public class MovieUpdateHandler : DbService<Movie>,
    IRequestHandler<MovieUpdateRequest, CommandResponse>
{
    private readonly MoviesDb _moviesDb;

    public MovieUpdateHandler(MoviesDb db) : base(db)
    {
        _moviesDb = db;
    }

    protected override IQueryable<Movie> DbQuery()
        => base.DbQuery().Include(movie => movie.MovieGenres);

    public async Task<CommandResponse> Handle(
        MovieUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var movie = await DbSingleAsync(request.Id, cancellationToken);

        if (movie is null)
            return Error("Movie not found!");

        if (!await _moviesDb.Directors.AnyAsync(
                director => director.Id == request.DirectorId,
                cancellationToken))
        {
            return Error("Director not found!");
        }

        var genreIds = (request.GenreIds ?? new List<int>())
            .Distinct()
            .ToList();

        var existingGenreCount = await _moviesDb.Genres
            .CountAsync(genre => genreIds.Contains(genre.Id), cancellationToken);

        if (existingGenreCount != genreIds.Count)
            return Error("One or more genres were not found!");

        movie.Name = request.Name.Trim();
        movie.ReleaseDate = request.ReleaseDate;
        movie.TotalRevenue = request.TotalRevenue;
        movie.DirectorId = request.DirectorId;

        movie.MovieGenres.Clear();
        foreach (var genreId in genreIds)
        {
            movie.MovieGenres.Add(new MovieGenre { GenreId = genreId });
        }

        await DbUpdateAsync(movie, cancellationToken);

        return Success(movie.Id, "Movie updated successfully.");
    }
}