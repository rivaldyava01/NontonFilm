using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cities.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Application.Cities.Queries.GetListCities;

public class GetCitiesForUserQuery : IRequest<ListResponse<GetCities_CityForUser>>
{

}

public class GetCitiesForUserMapping : IMapFrom<City, GetCities_CityForUser>
{
}

public class GetCitiesForUserQueryHandler : IRequestHandler<GetCitiesForUserQuery, ListResponse<GetCities_CityForUser>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCitiesForUserQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetCities_CityForUser>> Handle(GetCitiesForUserQuery request, CancellationToken cancellationToken)
    {
        var cities = _context.Cities
        .AsNoTracking()
            .Where(x => !x.IsDeleted & x.Cinemas.Any())
            .Include(a => a.Cinemas);

        var result = await cities
        .ProjectTo<GetCities_CityForUser>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
