using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Cinema.Queries.GetListCinemas;

namespace Zeta.NontonFilm.Application.Cinemas.Queries.GetListCinemas;

public class GetListCinemasQuery : IRequest<ListResponse<GetListCinemas_Cinema>>
{

}

public class GetListCinemaQueryHandler : IRequestHandler<GetListCinemasQuery, ListResponse<GetListCinemas_Cinema>>
{
    private readonly INontonFilmDbContext _context;

    public GetListCinemaQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ListResponse<GetListCinemas_Cinema>> Handle(GetListCinemasQuery request, CancellationToken cancellationToken)
    {
        var response = new ListResponse<GetListCinemas_Cinema>();

        var cinemas = await _context.Cinemas
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .ToListAsync(cancellationToken);

        foreach (var cinema in cinemas)
        {
            response.Items.Add(new GetListCinemas_Cinema
            {
                Id = cinema.Id,
                Name = cinema.Name
            });
        }

        return response;
    }
}
