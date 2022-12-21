using FluentValidation;

namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;

public class GetCinemasForUserRequest
{
    public Guid CityId { get; set; }
}
public class GetCinemaForUserRequestValidator : AbstractValidator<GetCinemasForUserRequest>
{
    public GetCinemaForUserRequestValidator()
    {
        RuleFor(x => x.CityId)
         .NotEmpty();
    }
}
