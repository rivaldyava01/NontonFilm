using FluentValidation;

using Zeta.NontonFilm.Shared.Genres.Constants;

namespace Zeta.NontonFilm.Shared.Genres.Commands.AddGenre;

public class AddGenreRequest
{
    public string Name { get; set; } = default!;

}

public class AddGenreRequestValidator : AbstractValidator<AddGenreRequest>
{
    public AddGenreRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);
    }
}
