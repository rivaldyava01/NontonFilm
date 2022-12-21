using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Audits.Options;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Audits.Queries.GetAudits;

[Authorize(Policy = Permissions.NontonFilm_Audit_Index)]
public class GetAuditsQuery : GetAuditsRequest, IRequest<PaginatedListResponse<GetAuditsAudit>>
{
}

public class GetAuditsQueryValidator : AbstractValidator<GetAuditsQuery>
{
    public GetAuditsQueryValidator(IOptions<AuditOptions> auditOptions)
    {
        Include(new GetAuditsRequestValidator(auditOptions));
    }
}

public class GetAuditsAuditMapping : IMapFrom<Audit, GetAuditsAudit>
{
}

public class GetAuditsQueryHandler : IRequestHandler<GetAuditsQuery, PaginatedListResponse<GetAuditsAudit>>
{
    private readonly INontonFilmDbContext _context;
    private readonly AuditOptions _auditOptions;
    private readonly IMapper _mapper;

    public GetAuditsQueryHandler(INontonFilmDbContext context, IOptions<AuditOptions> auditOptions, IMapper mapper)
    {
        _context = context;
        _auditOptions = auditOptions.Value;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetAuditsAudit>> Handle(GetAuditsQuery request, CancellationToken cancellationToken)
    {
        var from = request.From ?? _auditOptions.FilterMinimumCreated;
        var to = request.To ?? _auditOptions.FilterMaximumCreated;

        var query = _context.Audits
            .AsNoTracking()
            .Where(x => x.Created >= from && x.Created <= to)
            .ApplySearch(request.SearchText, typeof(GetAuditsAudit), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
                typeof(GetAuditsAudit),
                _mapper.ConfigurationProvider,
                nameof(GetAuditsAudit.Created),
                SortOrder.Desc);

        var result = await query
            .ProjectTo<GetAuditsAudit>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
