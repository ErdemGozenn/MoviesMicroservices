using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Genres;

public class GenreUpdateRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public List<int> MovieIds { get; set; } = new();
}

public class GenreUpdateHandler : DbService<Genre>,
    IRequestHandler<GenreUpdateRequest, CommandResponse>
{
    private readonly MoviesDb _moviesDb;

    public GenreUpdateHandler(MoviesDb db) : base(db)
    {
        _moviesDb = db;
    }

    protected override IQueryable<Genre> DbQuery()
        => base.DbQuery().Include(genre => genre.MovieGenres);

    public async Task<CommandResponse> Handle(
        GenreUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var genre = await DbSingleAsync(request.Id, cancellationToken);

        if (genre is null)
            return Error("Genre not found!");

        var movieIds = (request.MovieIds ?? new List<int>())
            .Distinct()
            .ToList();

        var existingMovieCount = await _moviesDb.Movies
            .CountAsync(movie => movieIds.Contains(movie.Id), cancellationToken);

        if (existingMovieCount != movieIds.Count)
            return Error("One or more movies were not found!");

        genre.Name = request.Name.Trim();
        genre.MovieGenres.Clear();

        foreach (var movieId in movieIds)
            genre.MovieGenres.Add(new MovieGenre { MovieId = movieId });

        await DbUpdateAsync(genre, cancellationToken);

        return Success(genre.Id, "Genre updated successfully.");
    }
}