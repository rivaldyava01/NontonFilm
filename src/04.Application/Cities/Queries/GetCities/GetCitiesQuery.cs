using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cities.Queries.GetCities;
using Zeta.NontonFilm.Shared.Cities.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Cities.Queries.GetCities;

[Authorize(Policy = Permissions.NontonFilm_City_Index)]

public class GetCitiesQuery : GetCitiesRequest, IRequest<PaginatedListResponse<GetCities_City>>
{

}

public class GetCitiesCityMapping : IMapFrom<City, GetCities_City>
{
}

public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, PaginatedListResponse<GetCities_City>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCitiesQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetCities_City>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Cities
            .AsNoTracking()
           .Where(x => !x.IsDeleted)
            .ApplySearch(request.SearchText, typeof(GetCities_City), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetCities_City),
            _mapper.ConfigurationProvider,
            nameof(GetCities_City.Name),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetCities_City>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
