using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Audits.Constants;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudit;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Audits.Queries.GetAudit;

[Authorize(Policy = Permissions.NontonFilm_Audit_View)]
public class GetAuditQuery : IRequest<GetAuditResponse>
{
    public Guid AuditId { get; set; }
}

public class GetAuditResponseMapping : IMapFrom<Audit, GetAuditResponse>
{
}

public class GetAuditQueryHandler : IRequestHandler<GetAuditQuery, GetAuditResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetAuditQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAuditResponse> Handle(GetAuditQuery request, CancellationToken cancellationToken)
    {
        var audit = await _context.Audits
            .AsNoTracking()
            .Where(x => x.Id == request.AuditId)
            .SingleOrDefaultAsync(cancellationToken);

        if (audit is null)
        {
            throw new NotFoundException(DisplayTextFor.Audit, request.AuditId);
        }

        return _mapper.Map<GetAuditResponse>(audit);
    }
}
