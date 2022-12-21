using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Genres.Queries.GetGenre;

[Authorize(Policy = Permissions.NontonFilm_Genre_View)]

public class GetGenreQuery : GetGenreRequest, IRequest<GetGenreResponse>
{

}

public class GetGenreResponseMapping : IMapFrom<Genre, GetGenreResponse>
{
}

public class GetMovieGenreResponseMapping : IMapFrom<MovieGenre, GetGenreResponse_MovieGenre>
{
}

public class GetGenreQueryHandler : IRequestHandler<GetGenreQuery, GetGenreResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetGenreQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetGenreResponse> Handle(GetGenreQuery request, CancellationToken cancellationToken)
    {
        var genre = await _context.Genres
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.MovieGenres)
                .ThenInclude(b => b.Movie)
            .SingleOrDefaultAsync(cancellationToken);

        if (genre is null)
        {
            throw new NotFoundException(DisplayTextFor.Genre, request.Id);
        }

        return _mapper.Map<GetGenreResponse>(genre);
    }
}
