using FluentValidation;

namespace Zeta.NontonFilm.Shared.Movies.Commands.DeleteMovie;

public class DeleteMovieRequest
{
    public Guid Id { get; set; }
}

public class DeleteMovieRequestValidator : AbstractValidator<DeleteMovieRequest>
{
    public DeleteMovieRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
