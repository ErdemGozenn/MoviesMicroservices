using MediatR;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Genres;

public class GenreRemoveRequest : Record, IRequest<CommandResponse>
{
}

public class GenreRemoveHandler : DbService<Genre>,
    IRequestHandler<GenreRemoveRequest, CommandResponse>
{
    public GenreRemoveHandler(MoviesDb db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(
        GenreRemoveRequest request,
        CancellationToken cancellationToken)
    {
        var genre = await DbSingleAsync(request.Id, cancellationToken);

        if (genre is null)
            return Error("Genre not found!");

        await DbRemoveAsync(genre, cancellationToken);

        return Success(genre.Id, "Genre deleted successfully.");
    }
}