using FluentValidation;
using Zeta.NontonFilm.Shared.Common.Requests;

namespace Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;

public class GetPastShowsRequest : PaginatedListRequest
{
    public Guid StudioId { get; set; }
}

public class GetPastShowRequestValidator : AbstractValidator<GetPastShowsRequest>
{
    public GetPastShowRequestValidator()
    {
        RuleFor(x => x.StudioId)
            .NotEmpty();
    }
}
