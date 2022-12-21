using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Cities.Queries.GetCity;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cities.Queries.GetCity;

[Authorize(Policy = Permissions.NontonFilm_City_View)]

public class GetCityQuery : GetCityRequest, IRequest<GetCityResponse>
{

}

public class GetCityResponseMapping : IMapFrom<City, GetCityResponse>
{
}

public class GetCinemaCityResponseMapping : IMapFrom<Cinema, GetCityResponse_Cinema>
{
}

public class GetCityQueryHandler : IRequestHandler<GetCityQuery, GetCityResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCityQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCityResponse> Handle(GetCityQuery request, CancellationToken cancellationToken)
    {
        var city = await _context.Cities
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.Cinemas)
            .SingleOrDefaultAsync(cancellationToken);

        if (city is null)
        {
            throw new NotFoundException(DisplayTextFor.City, request.Id);
        }

        return _mapper.Map<GetCityResponse>(city);
    }
}
