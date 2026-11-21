using System.ComponentModel.DataAnnotations;
using MediatR;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Directors;

public class DirectorAddRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public bool IsRetired { get; set; }
}

public class DirectorAddHandler : DbService<Director>,
    IRequestHandler<DirectorAddRequest, CommandResponse>
{
    public DirectorAddHandler(MoviesDb db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(
        DirectorAddRequest request,
        CancellationToken cancellationToken)
    {
        var director = new Director
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            IsRetired = request.IsRetired
        };

        await DbAddAsync(director, cancellationToken);

        return Success(director.Id, "Director created successfully.");
    }
}