using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Services;

namespace Movies.APP.Features.Genres;

public class GenreQueryRequest
    : Record, IRequest<IQueryable<GenreQueryResponse>>
{
}

public class GenreQueryResponse : Record
{
    public string Name { get; set; } = string.Empty;
    public List<int> MovieIds { get; set; } = new();
}

public class GenreQueryHandler : DbService<Genre>,
    IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
{
    public GenreQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<GenreQueryResponse>> Handle(
        GenreQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = DbQuery().Select(genre => new GenreQueryResponse
        {
            Id = genre.Id,
            Name = genre.Name,
            MovieIds = genre.MovieGenres
                .Select(movieGenre => movieGenre.MovieId)
                .ToList()
        });

        return Task.FromResult(query);
    }
}