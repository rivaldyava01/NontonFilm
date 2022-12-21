using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Enums;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Shows.Queries.GetUpcomingShows;

namespace Zeta.NontonFilm.Shows.Queries.GetUpcomingShows;

[Authorize(Policy = Permissions.NontonFilm_Show_Index_Upcoming)]

public class GetUpcomingShowsQuery : GetUpcomingShowsRequest, IRequest<PaginatedListResponse<GetUpcomingShows_Show>>
{

}

public class GetUpcomingShowsShowMapping : IMapFrom<Show, GetUpcomingShows_Show>
{
}

public class GetUpcomingShowsQueryHandler : IRequestHandler<GetUpcomingShowsQuery, PaginatedListResponse<GetUpcomingShows_Show>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetUpcomingShowsQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetUpcomingShows_Show>> Handle(GetUpcomingShowsQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Shows
            .AsNoTracking()
           .Where(x => !x.IsDeleted && x.StudioId == request.StudioId && x.ShowDateTime > DateTime.Now)
           .Include(a => a.Studio)
            .Include(b => b.Movie)
            .ApplySearch(request.SearchText, typeof(GetUpcomingShows_Show), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetUpcomingShows_Show),
            _mapper.ConfigurationProvider,
            nameof(GetUpcomingShows_Show.TicketPrice),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetUpcomingShows_Show>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
