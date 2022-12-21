using FluentValidation;
using Zeta.NontonFilm.Shared.Cities.Constants;

namespace Zeta.NontonFilm.Shared.Cities.Commands.UpdateCity;

public class UpdateCityRequest
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

}

public class UpdateCityRequestValidator : AbstractValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);
    }
}
