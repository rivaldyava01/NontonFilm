using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChain.Queries.GetListCinemaChains;

namespace Zeta.NontonFilm.Application.CinemaChains.Queries.GetListCinemaChains;

public class GetListCinemaChainsQuery : IRequest<ListResponse<GetListCinemaChains_CinemaChain>>
{

}

public class GetListCinemaChainQueryHandler : IRequestHandler<GetListCinemaChainsQuery, ListResponse<GetListCinemaChains_CinemaChain>>
{
    private readonly INontonFilmDbContext _context;

    public GetListCinemaChainQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ListResponse<GetListCinemaChains_CinemaChain>> Handle(GetListCinemaChainsQuery request, CancellationToken cancellationToken)
    {
        var response = new ListResponse<GetListCinemaChains_CinemaChain>();

        var cinemaChains = await _context.CinemaChains
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var cinemaChain in cinemaChains)
        {
            response.Items.Add(new GetListCinemaChains_CinemaChain
            {
                Id = cinemaChain.Id,
                Name = cinemaChain.Name
            });
        }

        return response;
    }
}
