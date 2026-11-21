using System.ComponentModel.DataAnnotations;
using MediatR;
using Movies.APP.Domain;
using Movies.Core.Domain;
using Movies.Core.Models;
using Movies.Core.Services;

namespace Movies.APP.Features.Genres;

public class GenreAddRequest : Record, IRequest<CommandResponse>
{
    [Required]
    public string Name { get; set; } = string.Empty;
}

public class GenreAddHandler : DbService<Genre>,
    IRequestHandler<GenreAddRequest, CommandResponse>
{
    public GenreAddHandler(MoviesDb db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(
        GenreAddRequest request,
        CancellationToken cancellationToken)
    {
        var genre = new Genre
        {
            Name = request.Name.Trim()
        };

        await DbAddAsync(genre, cancellationToken);

        return Success(genre.Id, "Genre created successfully.");
    }
}