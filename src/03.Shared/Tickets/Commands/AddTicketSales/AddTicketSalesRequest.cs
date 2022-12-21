using FluentValidation;
using Zeta.NontonFilm.Shared.Common.Attributes;
using Zeta.NontonFilm.Shared.Tickets.Constants;

namespace Zeta.NontonFilm.Shared.Tickets.Commands.AddTicketSales;

public class AddTicketSalesRequest
{

    public Guid ShowId { get; set; }

    [SpecialValueAttribute(SpecialValueType.Json)]
    public List<string> SeatCode { get; set; } = new();
}

public class AddTicketSalesRequestValidator : AbstractValidator<AddTicketSalesRequest>
{
    public AddTicketSalesRequestValidator()
    {
        RuleFor(x => x.ShowId)
           .NotEmpty();

        RuleFor(x => x.SeatCode.Count)
      .GreaterThanOrEqualTo(MinimumValueFor.SeatCode);
    }
}
