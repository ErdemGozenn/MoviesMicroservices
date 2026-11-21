using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Services;

namespace Movies.APP.Features.Movies;

public class MovieQueryRequest : Record, IRequest<IQueryable<MovieQueryResponse>>
{
}

public class MovieQueryResponse : Record
{
    public string Name { get; set; } = string.Empty;
    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int DirectorId { get; set; }
    public List<int> GenreIds { get; set; } = new();
}

public class MovieQueryHandler : DbService<Movie>,
    IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
{
    public MovieQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<MovieQueryResponse>> Handle(
        MovieQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = DbQuery().Select(movie => new MovieQueryResponse
        {
            Id = movie.Id,
            Name = movie.Name,
            ReleaseDate = movie.ReleaseDate,
            TotalRevenue = movie.TotalRevenue,
            DirectorId = movie.DirectorId,
            GenreIds = movie.MovieGenres
            .Select(movieGenre => movieGenre.GenreId)
            .ToList()
        });

        return Task.FromResult(query);
    }
}