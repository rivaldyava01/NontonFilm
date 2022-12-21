using FluentValidation;

namespace Zeta.NontonFilm.Shared.Shows.Queries.GetShow;

public class GetShowRequest
{
    public Guid Id { get; set; }
}
public class GetShowRequestValidator : AbstractValidator<GetShowRequest>
{
    public GetShowRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();
    }
}
