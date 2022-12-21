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
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Movies.Queries.GetMovies;

[Authorize(Policy = Permissions.NontonFilm_Movie_Index)]

public class GetMoviesQuery : GetMoviesRequest, IRequest<PaginatedListResponse<GetMovies_Movie>>
{

}

public class GetMoviesMovieMapping : IMapFrom<Movie, GetMovies_Movie>
{
}

public class GetMoviesGenreMapping : IMapFrom<MovieGenre, GetMovies_MovieGenre>
{
}

public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, PaginatedListResponse<GetMovies_Movie>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetMoviesQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedListResponse<GetMovies_Movie>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Movies
            .AsNoTracking()
           .Where(x => !x.IsDeleted)
           .Include(a => a.MovieGenres)
                .ThenInclude(b => b.Genre)
            .ApplySearch(request.SearchText, typeof(GetMovies_Movie), _mapper.ConfigurationProvider)
            .ApplyOrder(request.SortField, request.SortOrder,
            typeof(GetMovies_Movie),
            _mapper.ConfigurationProvider,
            nameof(GetMovies_Movie.Title),
            SortOrder.Desc);

        var result = await query
            .ProjectTo<GetMovies_Movie>(_mapper.ConfigurationProvider)
            .ToPaginatedListAsync(request.Page, request.PageSize, cancellationToken);

        return result.ToPaginatedListResponse();
    }
}
