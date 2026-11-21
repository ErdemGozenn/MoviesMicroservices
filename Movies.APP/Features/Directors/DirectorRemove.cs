using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Directors;

public class DirectorRemoveRequest : Record, IRequest<CommandResponse>
{
}

public class DirectorRemoveHandler : DbService<Director>,
    IRequestHandler<DirectorRemoveRequest, CommandResponse>
{
    private readonly MoviesDb _moviesDb;

    public DirectorRemoveHandler(MoviesDb db) : base(db)
    {
        _moviesDb = db;
    }

    public async Task<CommandResponse> Handle(
        DirectorRemoveRequest request,
        CancellationToken cancellationToken)
    {
        var director = await DbSingleAsync(request.Id, cancellationToken);

        if (director is null)
            return Error("Director not found!");

        var hasMovies = await _moviesDb.Movies
            .AnyAsync(movie => movie.DirectorId == request.Id, cancellationToken);

        if (hasMovies)
            return Error("Director has movies and cannot be deleted.");

        await DbRemoveAsync(director, cancellationToken);

        return Success(director.Id, "Director deleted successfully.");
    }
}