using FluentValidation;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Shared.Audits.Options;
using Zeta.NontonFilm.Shared.Common.Requests;

namespace Zeta.NontonFilm.Shared.Audits.Queries.GetAudits;

public class GetAuditsRequest : PaginatedListRequest
{
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}

public class GetAuditsRequestValidator : AbstractValidator<GetAuditsRequest>
{
    private readonly DateTime _fromTimestamp;
    private readonly DateTime _toTimestamp;

    public GetAuditsRequestValidator(IOptions<AuditOptions> auditOptions)
    {
        _fromTimestamp = auditOptions.Value.FilterMinimumCreated;
        _toTimestamp = auditOptions.Value.FilterMaximumCreated;

        Include(new PaginatedListRequestValidator());

        When(v => v.From is not null, () =>
        {
            RuleFor(v => v.From).InclusiveBetween(_fromTimestamp, _toTimestamp);
            RuleFor(v => v.From).LessThanOrEqualTo(x => x.To);
        });

        When(v => v.To is not null, () =>
        {
            RuleFor(v => v.To).InclusiveBetween(_fromTimestamp, _toTimestamp);
            RuleFor(v => v.To).GreaterThanOrEqualTo(x => x.From);
        });
    }
}
