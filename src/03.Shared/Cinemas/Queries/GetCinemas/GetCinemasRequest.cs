using FluentValidation;
using Zeta.NontonFilm.Shared.Common.Requests;

namespace Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;
public class GetCinemasRequest : PaginatedListRequest
{
    public Guid CinemaChainId { get; set; }
}
public class GetCinemaRequestValidator : AbstractValidator<GetCinemasRequest>
{
    public GetCinemaRequestValidator()
    {
        RuleFor(x => x.CinemaChainId)
         .NotEmpty();
    }
}
