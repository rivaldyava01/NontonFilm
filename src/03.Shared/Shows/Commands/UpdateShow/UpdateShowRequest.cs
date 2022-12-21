using FluentValidation;

namespace Zeta.NontonFilm.Shared.Shows.Commands.UpdateShow;

public class UpdateShowRequest
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public Guid StudioId { get; set; }
    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }

}

public class UpdateShowRequestValidator : AbstractValidator<UpdateShowRequest>
{
    public UpdateShowRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

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
