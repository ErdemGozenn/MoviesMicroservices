using MediatR;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Movies;

public class MovieRemoveRequest : Record, IRequest<CommandResponse>
{
}

public class MovieRemoveHandler : DbService<Movie>,
    IRequestHandler<MovieRemoveRequest, CommandResponse>
{
    public MovieRemoveHandler(MoviesDb db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(
        MovieRemoveRequest request,
        CancellationToken cancellationToken)
    {
        var movie = await DbSingleAsync(request.Id, cancellationToken);

        if (movie is null)
            return Error("Movie not found!");

        await DbRemoveAsync(movie, cancellationToken);

        return Success(movie.Id, "Movie deleted successfully.");
    }
}