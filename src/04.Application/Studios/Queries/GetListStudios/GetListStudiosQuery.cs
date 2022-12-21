using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Studio.Queries.GetListStudios;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Application.Studios.Queries.GetListStudios;

public class GetListStudiosQuery : IRequest<ListResponse<GetListStudios_Studio>>
{

}

public class GetListStudioMapping : IMapFrom<Studio, GetListStudios_Studio>
{
}

public class GetListStudioQueryHandler : IRequestHandler<GetListStudiosQuery, ListResponse<GetListStudios_Studio>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetListStudioQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetListStudios_Studio>> Handle(GetListStudiosQuery request, CancellationToken cancellationToken)
    {
        var genres = _context.Studios
        .AsNoTracking()
            .Where(x => !x.IsDeleted);

        var result = await genres
        .ProjectTo<GetListStudios_Studio>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
