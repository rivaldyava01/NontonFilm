using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Shows.Constants;
using Zeta.NontonFilm.Shared.Shows.Queries.GetShow;

namespace Zeta.NontonFilm.Application.Shows.Queries.GetShow;

public class GetShowQuery : IRequest<GetShowResponse>
{
    public Guid Id { get; set; }
}

public class GetShowResponseMapping : IMapFrom<Show, GetShowResponse>
{
}

public class GetShowQueryHandler : IRequestHandler<GetShowQuery, GetShowResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetShowQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetShowResponse> Handle(GetShowQuery request, CancellationToken cancellationToken)
    {
        var movie = await _context.Shows
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.Studio)
            .Include(b => b.Movie)
            .SingleOrDefaultAsync(cancellationToken);

        if (movie is null)
        {
            throw new NotFoundException(DisplayTextFor.Show, request.Id);
        }

        return _mapper.Map<GetShowResponse>(movie);
    }
}
