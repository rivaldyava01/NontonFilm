using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Studios.Queries.GetStudios;

[Authorize(Policy = Permissions.NontonFilm_Studio_Index)]

public class GetStudiosQuery : GetStudiosRequest, IRequest<PaginatedListResponse<GetStudios_Studio>>
{

}

public class GetStudiosStudioMapping : IMapFrom<Studio, GetStudios_Studio>
{
}

public class GetStudiosQueryHandler : IRequestHandler<GetStudiosQuery, PaginatedListResponse<GetStudios_Studio>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetStudiosQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetStudios_Studio>> Handle(GetStudiosQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Studios
            .AsNoTracking()
           .Where(x => !x.IsDeleted && x.CinemaId == request.CinemaId)
           .Include(a => a.Cinema)
           .Include(b => b.Seats)
            .ApplySearch(request.SearchText, typeof(GetStudios_Studio), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetStudios_Studio),
            _mapper.ConfigurationProvider,
            nameof(GetStudios_Studio.Name),
            SortOrder.Desc);



        var result = await query
            .ProjectTo<GetStudios_Studio>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        foreach (var item in result.Items)
        {
            var seats = await _context.Seats.Where(x => !x.IsDeleted && x.StudioId == item.Id).ToListAsync(cancellationToken);
            item.TotalSeat = seats.Count();
        }

        return result.ToPaginatedListResponse();
    }
}
