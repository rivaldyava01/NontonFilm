using FluentValidation;

using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Shared.Studios.Commands.AddStudio;

public class AddStudioRequest
{
    public string Name { get; set; } = default!;
    public Guid CinemaId { get; set; } = default!;
    public int Row { get; set; }
    public int Column { get; set; }

}

public class AddStudioRequestValidator : AbstractValidator<AddStudioRequest>
{
    public AddStudioRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);

        RuleFor(x => x.CinemaId)
            .NotEmpty();

        RuleFor(x => x.Row)
            .NotEmpty();

        RuleFor(x => x.Column)
            .NotEmpty();
    }
}
