using FluentValidation;

namespace Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;

public class GetStudioRequest
{
    public Guid Id { get; set; }
}
public class GetStudioRequestValidator : AbstractValidator<GetStudioRequest>
{
    public GetStudioRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();
    }
}
