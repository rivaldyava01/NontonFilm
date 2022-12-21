using FluentValidation;

namespace TokoPaEdi.Shared.CinemaChains.Commands.DeleteCinemaChain;

public class DeleteCinemaChainRequest
{
    public Guid Id { get; set; }
}

public class DeleteCinemaChainRequestValidator : AbstractValidator<DeleteCinemaChainRequest>
{
    public DeleteCinemaChainRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
