using FluentValidation;

namespace Zeta.NontonFilm.Shared.Seats.Queries.GetSeat;

public class GetSeatRequest
{
    public Guid? StudioId { get; set; }
    public string Row { get; set; } = default!;
    public string Column { get; set; } = default!;
}

public class GetSeatRequestValidator : AbstractValidator<GetSeatRequest>
{
    public GetSeatRequestValidator()
    {
        RuleFor(x => x.StudioId)
            .NotEmpty();

        RuleFor(x => x.Row)
            .NotEmpty();

        RuleFor(x => x.Column)
            .NotEmpty();
    }
}
