using FluentValidation;
using Zeta.NontonFilm.Shared.Cinemas.Constants;

namespace Zeta.NontonFilm.Shared.Cinemas.Commands.UpdateCinema;

public class UpdateCinemaRequest
{
    public Guid Id { get; set; } = default!;
    public Guid CityId { get; set; } = default!;
    public Guid CinemaChainId { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

}

public class UpdateCinemaRequestValidator : AbstractValidator<UpdateCinemaRequest>
{
    public UpdateCinemaRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();

        RuleFor(x => x.CityId)
          .NotEmpty();

        RuleFor(x => x.CinemaChainId)
         .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(MinimumLengthFor.Name)
            .MaximumLength(MaximumLengthFor.Name);

        RuleFor(x => x.Address)
           .NotEmpty()
           .MinimumLength(MinimumLengthFor.Address)
            .MaximumLength(MaximumLengthFor.Address);

        RuleFor(x => x.EmailAddress)
           .NotEmpty()
           .MinimumLength(MinimumLengthFor.EmailAddress)
           .MaximumLength(MaximumLengthFor.EmailAddress);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
             .MinimumLength(MinimumLengthFor.PhoneNumber)
            .MaximumLength(MaximumLengthFor.PhoneNumber);
    }
}
