using FluentValidation;

namespace Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;
public class GetGenreRequest
{
    public Guid Id { get; set; }
}
public class GetGenreRequestValidator : AbstractValidator<GetGenreRequest>
{
    public GetGenreRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();
    }
}
