using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Movies.Queries.GetMovie;

[Authorize(Policy = Permissions.NontonFilm_Movie_View)]

public class GetMovieQuery : IRequest<GetMovieResponse>
{
    public Guid Id { get; set; }
}

public class GetMovieResponseMapping : IMapFrom<Movie, GetMovieResponse>
{
}

public class GetMovieGenreResponseMapping : IMapFrom<MovieGenre, GetMovieResponse_MovieGenre>
{
}

public class GetMovieQueryHandler : IRequestHandler<GetMovieQuery, GetMovieResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetMovieQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetMovieResponse> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.MovieGenres)
                .ThenInclude(b => b.Genre)
            .SingleOrDefaultAsync(cancellationToken);

        if (movie is null)
        {
            throw new NotFoundException(DisplayTextFor.Movie, request.Id);
        }

        return _mapper.Map<GetMovieResponse>(movie);
    }
}
