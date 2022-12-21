using FluentValidation;

namespace Zeta.NontonFilm.Shared.Cinemas.Commands.DeleteCinema;

public class DeleteCinemaRequest
{
    public Guid Id { get; set; }
}

public class DeleteCinemaRequestValidator : AbstractValidator<DeleteCinemaRequest>
{
    public DeleteCinemaRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
