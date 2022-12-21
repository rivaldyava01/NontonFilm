using FluentValidation;

namespace Zeta.NontonFilm.Shared.Shows.Commands.AddShow;

public class AddShowRequest
{
    public Guid MovieId { get; set; }
    public Guid StudioId { get; set; }
    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }

}

public class AddShowRequestValidator : AbstractValidator<AddShowRequest>
{
    public AddShowRequestValidator()
    {
        RuleFor(x => x.MovieId)
            .NotEmpty();

        RuleFor(x => x.StudioId)
          .NotEmpty();

        RuleFor(x => x.ShowDateTime)
          .NotEmpty();

        RuleFor(x => x.TicketPrice)
          .NotEmpty();
    }
}
