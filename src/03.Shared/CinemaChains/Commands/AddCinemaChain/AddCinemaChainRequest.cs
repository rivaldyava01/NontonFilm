using FluentValidation;

using Zeta.NontonFilm.Shared.CinemaChains.Constants;

namespace Zeta.NontonFilm.Shared.CinemaChains.Commands.AddCinemaChain;

public class AddCinemaChainRequest
{
    public string Name { get; set; } = default!;
    public string OfficeAddress { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

}

public class AddCinemaChainRequestValidator : AbstractValidator<AddCinemaChainRequest>
{
    public AddCinemaChainRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);

        RuleFor(x => x.OfficeAddress)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.OfficeAddress)
            .MaximumLength(MaximumLengthFor.OfficeAddress);

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .EmailAddress()
            .MinimumLength(MinimumLengthFor.EmailAddress)
            .MaximumLength(MaximumLengthFor.EmailAddress);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.PhoneNumber)
            .MaximumLength(MaximumLengthFor.PhoneNumber);
    }
}
