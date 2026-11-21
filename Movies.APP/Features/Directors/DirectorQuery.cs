using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Services;

namespace Movies.APP.Features.Directors;

public class DirectorQueryRequest
    : Record, IRequest<IQueryable<DirectorQueryResponse>>
{
}

public class DirectorQueryResponse : Record
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsRetired { get; set; }
    public List<int> MovieIds { get; set; } = new();
}

public class DirectorQueryHandler : DbService<Director>,
    IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
{
    public DirectorQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<DirectorQueryResponse>> Handle(
        DirectorQueryRequest request,
        CancellationToken cancellationToken)
    {
        var query = DbQuery().Select(director => new DirectorQueryResponse
        {
            Id = director.Id,
            FirstName = director.FirstName,
            LastName = director.LastName,
            IsRetired = director.IsRetired,
            MovieIds = director.Movies
                .Select(movie => movie.Id)
                .ToList()
        });

        return Task.FromResult(query);
    }
}