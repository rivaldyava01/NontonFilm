using FluentValidation;

namespace TokoPaEdi.Shared.Cities.Commands.DeleteCity;

public class DeleteCityRequest
{
    public Guid Id { get; set; }
}

public class DeleteCityRequestValidator : AbstractValidator<DeleteCityRequest>
{
    public DeleteCityRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
