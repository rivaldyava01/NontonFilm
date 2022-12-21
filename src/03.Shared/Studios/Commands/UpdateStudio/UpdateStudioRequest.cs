using FluentValidation;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Shared.Studios.Commands.UpdateStudio;

public class UpdateStudioRequest
{
    public Guid Id { get; set; } = default!;
    public Guid CinemaId { get; set; } = default!;
    public string Name { get; set; } = default!;

}

public class UpdateStudioRequestValidator : AbstractValidator<UpdateStudioRequest>
{
    public UpdateStudioRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();

        RuleFor(x => x.Name)
        .NotEmpty()
        .MinimumLength(MinimumLengthFor.Name)
        .MaximumLength(MaximumLengthFor.Name);

        RuleFor(x => x.CinemaId)
            .NotEmpty();
    }
}
