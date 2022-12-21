using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Movie.Queries.GetListMovies;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Application.Movies.Queries.GetListMovies;

public class GetListMoviesQuery : IRequest<ListResponse<GetListMovies_Movie>>
{

}

public class GetListMovieMapping : IMapFrom<Movie, GetListMovies_Movie>
{
}

public class GetListMovieQueryHandler : IRequestHandler<GetListMoviesQuery, ListResponse<GetListMovies_Movie>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetListMovieQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetListMovies_Movie>> Handle(GetListMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = _context.Movies
        .AsNoTracking()
            .Where(x => !x.IsDeleted);

        var result = await movies
        .ProjectTo<GetListMovies_Movie>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
