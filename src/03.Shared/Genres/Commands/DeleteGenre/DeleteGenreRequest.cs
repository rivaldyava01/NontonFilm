using FluentValidation;

namespace TokoPaEdi.Shared.Genres.Commands.DeleteGenre;
public class DeleteGenreRequest
{
    public Guid Id { get; set; }
}

public class DeleteGenreRequestValidator : AbstractValidator<DeleteGenreRequest>
{
    public DeleteGenreRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
