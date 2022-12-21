using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Cinemas.Queries.GetCinemas;

[Authorize(Policy = Permissions.NontonFilm_Cinema_Index)]

public class GetCinemasQuery : GetCinemasRequest, IRequest<PaginatedListResponse<GetCinemas_Cinema>>
{

}

public class GetCinemasCinemaMapping : IMapFrom<Cinema, GetCinemas_Cinema>
{
}

public class GetCinemasQueryHandler : IRequestHandler<GetCinemasQuery, PaginatedListResponse<GetCinemas_Cinema>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCinemasQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetCinemas_Cinema>> Handle(GetCinemasQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cinemas
            .AsNoTracking()
           .Where(x => !x.IsDeleted && x.CinemaChainId == request.CinemaChainId)
           .Include(a => a.CinemaChain)
            .Include(b => b.City)
            .ApplySearch(request.SearchText, typeof(GetCinemas_Cinema), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetCinemas_Cinema),
            _mapper.ConfigurationProvider,
            nameof(GetCinemas_Cinema.Name),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetCinemas_Cinema>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
