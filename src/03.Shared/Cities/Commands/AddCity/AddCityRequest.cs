using FluentValidation;

using Zeta.NontonFilm.Shared.Cities.Constants;

namespace Zeta.NontonFilm.Shared.Cities.Commands.AddCity;

public class AddCityRequest
{
    public string Name { get; set; } = default!;

}

public class AddCityRequestValidator : AbstractValidator<AddCityRequest>
{
    public AddCityRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);
    }
}
