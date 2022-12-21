using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinema;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cinemas.Queries.GetCinema;

[Authorize(Policy = Permissions.NontonFilm_Cinema_View)]

public class GetCinemaQuery : IRequest<GetCinemaResponse>
{
    public Guid Id { get; set; }
}

public class GetCinemaResponseMapping : IMapFrom<Cinema, GetCinemaResponse>
{
}

public class GetCinemaStudioResponseMapping : IMapFrom<Studio, GetCinemaResponse_Studio>
{
}

public class GetCinemaQueryHandler : IRequestHandler<GetCinemaQuery, GetCinemaResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCinemaQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCinemaResponse> Handle(GetCinemaQuery request, CancellationToken cancellationToken)
    {
        var cinema = await _context.Cinemas
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.CinemaChain)
            .Include(b => b.City)
            .Include(c => c.Studios)
            .SingleOrDefaultAsync(cancellationToken);

        if (cinema is null)
        {
            throw new NotFoundException(DisplayTextFor.Cinema, request.Id);
        }

        return _mapper.Map<GetCinemaResponse>(cinema);
    }
}
