using FluentValidation;

namespace Zeta.NontonFilm.Shared.Cities.Queries.GetCity;

public class GetCityRequest
{
    public Guid Id { get; set; }
}
public class GetCityRequestValidator : AbstractValidator<GetCityRequest>
{
    public GetCityRequestValidator()
    {
        RuleFor(x => x.Id)
         .NotEmpty();
    }
}
