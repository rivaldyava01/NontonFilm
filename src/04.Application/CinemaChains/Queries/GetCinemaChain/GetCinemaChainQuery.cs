using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Common.Mappings;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChain;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.CinemaChains.Queries.GetCinemaChain;

[Authorize(Policy = Permissions.NontonFilm_CinemaChain_View)]

public class GetCinemaChainQuery : IRequest<GetCinemaChainResponse>
{
    public Guid Id { get; set; }
}

public class GetCinemaChainResponseMapping : IMapFrom<CinemaChain, GetCinemaChainResponse>
{
}

public class GetCinemaCinemaChainResponseMapping : IMapFrom<Cinema, GetCinemaChainResponse_Cinema>
{
}

public class GetCinemaChainQueryHandler : IRequestHandler<GetCinemaChainQuery, GetCinemaChainResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IMapper _mapper;

    public GetCinemaChainQueryHandler(INontonFilmDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCinemaChainResponse> Handle(GetCinemaChainQuery request, CancellationToken cancellationToken)
    {
        var cinemaChain = await _context.CinemaChains
            .Where(x => !x.IsDeleted && x.Id == request.Id)
            .Include(a => a.Cinemas)
            .SingleOrDefaultAsync(cancellationToken);

        if (cinemaChain is null)
        {
            throw new NotFoundException(DisplayTextFor.CinemaChain, request.Id);
        }

        return _mapper.Map<GetCinemaChainResponse>(cinemaChain);
    }
}
