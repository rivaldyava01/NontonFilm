using FluentValidation;
using Zeta.NontonFilm.Shared.Common.Enums;

namespace Zeta.NontonFilm.Shared.Common.Requests;

public class PaginatedListRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SearchText { get; set; }
    public string? SortField { get; set; }
    public SortOrder? SortOrder { get; set; }
}

public class PaginatedListRequestValidator : AbstractValidator<PaginatedListRequest>
{
    public PaginatedListRequestValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThan(0);

        RuleFor(v => v.PageSize)
            .GreaterThan(0)
            .LessThanOrEqualTo(100);

        RuleFor(v => v.SortOrder)
            .IsInEnum();
    }
}
