using FluentValidation;

namespace Zeta.NontonFilm.Shared.Studios.Commands.DeleteStudio;

public class DeleteStudioRequest
{
    public Guid Id { get; set; }
}

public class DeleteStudioRequestValidator : AbstractValidator<DeleteStudioRequest>
{
    public DeleteStudioRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
