using System.ComponentModel.DataAnnotations;
using MediatR;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Directors;

public class DirectorUpdateRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public bool IsRetired { get; set; }
}

public class DirectorUpdateHandler : DbService<Director>,
    IRequestHandler<DirectorUpdateRequest, CommandResponse>
{
    public DirectorUpdateHandler(MoviesDb db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(
        DirectorUpdateRequest request,
        CancellationToken cancellationToken)
    {
        var director = await DbSingleAsync(request.Id, cancellationToken);

        if (director is null)
            return Error("Director not found!");

        director.FirstName = request.FirstName.Trim();
        director.LastName = request.LastName.Trim();
        director.IsRetired = request.IsRetired;

        await DbUpdateAsync(director, cancellationToken);

        return Success(director.Id, "Director updated successfully.");
    }
}