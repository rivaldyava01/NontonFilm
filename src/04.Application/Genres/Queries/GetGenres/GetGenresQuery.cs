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
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenres;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Genres.Queries.GetGenres;

[Authorize(Policy = Permissions.NontonFilm_Genre_Index)]

public class GetGenresQuery : GetGenresRequest, IRequest<PaginatedListResponse<GetGenres_Genre>>
{

}

public class GetGenresGenreMapping : IMapFrom<Genre, GetGenres_Genre>
{
}

public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, PaginatedListResponse<GetGenres_Genre>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetGenresQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetGenres_Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Genres
            .AsNoTracking()
           .Where(x => !x.IsDeleted)
            .ApplySearch(request.SearchText, typeof(GetGenres_Genre), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetGenres_Genre),
            _mapper.ConfigurationProvider,
            nameof(GetGenres_Genre.Name),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetGenres_Genre>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
