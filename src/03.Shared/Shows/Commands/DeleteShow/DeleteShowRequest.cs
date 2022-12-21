using FluentValidation;

namespace TokoPaEdi.Shared.Shows.Commands.DeleteShow;
public class DeleteShowRequest
{
    public Guid Id { get; set; }
}

public class DeleteShowRequestValidator : AbstractValidator<DeleteShowRequest>
{
    public DeleteShowRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
