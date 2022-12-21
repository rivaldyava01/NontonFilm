using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovies;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Queries.GetNowShowingMovies;

[Authorize(Policy = Permissions.NontonFilm_Movie_NowShowing)]

public class GetNowShowingMoviesQuery : IRequest<ListResponse<GetNowShowingMovies_Movie>>
{

}

public class GetNowShowingMovieMapping : IMapFrom<Movie, GetNowShowingMovies_Movie>
{
}

public class GetNowShowingMovieQueryHandler : IRequestHandler<GetNowShowingMoviesQuery, ListResponse<GetNowShowingMovies_Movie>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetNowShowingMovieQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetNowShowingMovies_Movie>> Handle(GetNowShowingMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = _context.Movies
        .AsNoTracking()
            .Where(x => !x.IsDeleted && x.Shows.Any());

        var result = await movies
        .ProjectTo<GetNowShowingMovies_Movie>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
