using FluentValidation;
using Zeta.NontonFilm.Shared.Common.Requests;

namespace Zeta.NontonFilm.Shared.Shows.Queries.GetUpcomingShows;

public class GetUpcomingShowsRequest : PaginatedListRequest
{
    public Guid StudioId { get; set; }
}

public class GetUpcomingShowRequestValidator : AbstractValidator<GetUpcomingShowsRequest>
{
    public GetUpcomingShowRequestValidator()
    {
        RuleFor(x => x.StudioId)
            .NotEmpty();
    }
}
