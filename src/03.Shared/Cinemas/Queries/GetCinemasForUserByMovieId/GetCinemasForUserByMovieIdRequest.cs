using FluentValidation;

namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;

public class GetCinemasForUserByMovieIdRequest
{
    public Guid CityId { get; set; }
    public Guid MovieId { get; set; }
}
public class GetCinemaForUserByMovieIdRequestValidator : AbstractValidator<GetCinemasForUserByMovieIdRequest>
{
    public GetCinemaForUserByMovieIdRequestValidator()
    {
        RuleFor(x => x.CityId)
         .NotEmpty();
    }
}
