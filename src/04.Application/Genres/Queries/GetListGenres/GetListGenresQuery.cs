using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Extensions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Genre.Queries.GetListGenres;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Application.Genres.Queries.GetListGenres;

public class GetListGenresQuery : IRequest<ListResponse<GetListGenres_Genre>>
{

}

public class GetListGenreMapping : IMapFrom<Genre, GetListGenres_Genre>
{
}

public class GetListGenreQueryHandler : IRequestHandler<GetListGenresQuery, ListResponse<GetListGenres_Genre>>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetListGenreQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListResponse<GetListGenres_Genre>> Handle(GetListGenresQuery request, CancellationToken cancellationToken)
    {
        var genres = _context.Genres
        .AsNoTracking()
            .Where(x => !x.IsDeleted);

        var result = await genres
        .ProjectTo<GetListGenres_Genre>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);

        return result.ToListResponse();
    }
}
