using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Studios.Constants;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;

namespace Zeta.NontonFilm.Application.Studios.Queries.GetStudio;

[Authorize(Policy = Permissions.NontonFilm_Studio_View)]

public class GetStudioQuery : GetStudioRequest, IRequest<GetStudioResponse>
{

}

public class GetStudioResponseMapping : IMapFrom<Studio, GetStudioResponse>
{
}

public class GetStudioQueryHandler : IRequestHandler<GetStudioQuery, GetStudioResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetStudioQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetStudioResponse> Handle(GetStudioQuery request, CancellationToken cancellationToken)
    {
        var studio = await _context.Studios
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.Cinema)
            .SingleOrDefaultAsync(cancellationToken);

        if (studio is null)
        {
            throw new NotFoundException(DisplayTextFor.Studio, request.Id);
        }

        return _mapper.Map<GetStudioResponse>(studio);
    }
}
