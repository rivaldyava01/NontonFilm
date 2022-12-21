using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Queries.GetMovie;

[Authorize(Policy = Permissions.NontonFilm_Movie_NowShowing)]

public class GetNowShowingMovieQuery : IRequest<GetNowShowingMovieResponse>
{
    public Guid Id { get; set; }
}

public class GetNowShowingMovieResponseMapping : IMapFrom<Movie, GetNowShowingMovieResponse>
{
}

public class GetNowShowingMovieGenreResponseMapping : IMapFrom<MovieGenre, GetNowShowingMovieResponse_Genre>
{
}

public class GetNowShowingMovieQueryHandler : IRequestHandler<GetNowShowingMovieQuery, GetNowShowingMovieResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetNowShowingMovieQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetNowShowingMovieResponse> Handle(GetNowShowingMovieQuery request, CancellationToken cancellationToken)
    {
        var response = new GetNowShowingMovieResponse();

        var movie = await _context.Movies
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.MovieGenres)
                .ThenInclude(b => b.Genre)
            .SingleOrDefaultAsync(cancellationToken);

        if (movie is null)
        {
            throw new NotFoundException(DisplayTextFor.Movie, request.Id);
        }

        return _mapper.Map<GetNowShowingMovieResponse>(movie);
    }
}
