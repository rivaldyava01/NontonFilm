using FluentValidation;
using Zeta.NontonFilm.Shared.Genres.Constants;

namespace Zeta.NontonFilm.Shared.Genres.Commands.UpdateGenre;

public class UpdateGenreRequest
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

}

public class UpdateGenreRequestValidator : AbstractValidator<UpdateGenreRequest>
{
    public UpdateGenreRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);
    }
}
