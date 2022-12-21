using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChains;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.CinemaChains.Queries.GetCinemaChains;

[Authorize(Policy = Permissions.NontonFilm_CinemaChain_Index)]

public class GetCinemaChainsQuery : GetCinemaChainsRequest, IRequest<PaginatedListResponse<GetCinemaChains_CinemaChain>>
{

}

public class GetCinemaChainsCinemaChainMapping : IMapFrom<CinemaChain, GetCinemaChains_CinemaChain>
{
}

public class GetCinemaChainsQueryHandler : IRequestHandler<GetCinemaChainsQuery, PaginatedListResponse<GetCinemaChains_CinemaChain>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCinemaChainsQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetCinemaChains_CinemaChain>> Handle(GetCinemaChainsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.CinemaChains
             .AsNoTracking()
            .Where(x => !x.IsDeleted)
             .ApplySearch(request.SearchText, typeof(GetCinemaChains_CinemaChain), _mapper.ConfigurationProvider)
             .ApplyOrder(request.SortField, request.SortOrder,
             typeof(GetCinemaChains_CinemaChain),
             _mapper.ConfigurationProvider,
             nameof(GetCinemaChains_CinemaChain.Name),
             SortOrder.Desc);

        var result = await query
            .ProjectTo<GetCinemaChains_CinemaChain>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();

    }
}
