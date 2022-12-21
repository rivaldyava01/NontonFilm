using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.City.Queries.GetListCities;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Application.Cities.Queries.GetListCities;

public class GetListCitiesQuery : IRequest<ListResponse<GetListCities_City>>
{

}

public class GetListCityMapping : IMapFrom<City, GetListCities_City>
{
}

public class GetListCityQueryHandler : IRequestHandler<GetListCitiesQuery, ListResponse<GetListCities_City>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetListCityQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetListCities_City>> Handle(GetListCitiesQuery request, CancellationToken cancellationToken)
    {
        var cities = _context.Cities
        .AsNoTracking()
            .Where(x => !x.IsDeleted);

        var result = await cities
        .ProjectTo<GetListCities_City>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
