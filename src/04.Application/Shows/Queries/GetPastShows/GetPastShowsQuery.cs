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
using Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;

namespace Zeta.NontonFilm.Shows.Queries.GetPastShows;

[Authorize(Policy = Permissions.NontonFilm_Show_Index_Past)]

public class GetPastShowsQuery : GetPastShowsRequest, IRequest<PaginatedListResponse<GetPastShows_Show>>
{

}

public class GetPastShowsShowMapping : IMapFrom<Show, GetPastShows_Show>
{
}

public class GetPastShowsQueryHandler : IRequestHandler<GetPastShowsQuery, PaginatedListResponse<GetPastShows_Show>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetPastShowsQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetPastShows_Show>> Handle(GetPastShowsQuery request, CancellationToken cancellationToken)
    {

        var query = _context.Shows
            .AsNoTracking()
           .Where(x => !x.IsDeleted && x.StudioId == request.StudioId && x.ShowDateTime < DateTime.Now)
           .Include(a => a.Studio)
            .Include(b => b.Movie)
            .ApplySearch(request.SearchText, typeof(GetPastShows_Show), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetPastShows_Show),
            _mapper.ConfigurationProvider,
            nameof(GetPastShows_Show.TicketPrice),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetPastShows_Show>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
