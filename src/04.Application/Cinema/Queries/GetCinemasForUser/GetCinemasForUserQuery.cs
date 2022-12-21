using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUser;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using static Zeta.NontonFilm.Shared.Shows.Constants.ApiEndpoint.V1;

namespace Zeta.NontonFilm.Application.Cinemas.Queries.GetCinemasForUser;

[Authorize(Policy = Permissions.NontonFilm_Cinema_User_Handle)]

public class GetCinemasForUserQuery : GetCinemasForUserRequest, IRequest<ListResponse<GetCinemasForUser_Cinema>>
{

}

public class GetCinemaForUserQueryHandler : IRequestHandler<GetCinemasForUserQuery, ListResponse<GetCinemasForUser_Cinema>>
{
    private readonly INontonFilmDbContext _context;

    public GetCinemaForUserQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ListResponse<GetCinemasForUser_Cinema>> Handle(GetCinemasForUserQuery request, CancellationToken cancellationToken)
    {
        var response = new ListResponse<GetCinemasForUser_Cinema>();

        var cinemasQuery = await _context.Cinemas
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.CityId == request.CityId && x.Studios.Any())
            .Include(a => a.Studios.Where(x => x.Shows.Any()))
                .ThenInclude(b => b.Shows.Where(b => b.ShowDateTime > DateTime.Now && !b.IsDeleted))
            .ToListAsync(cancellationToken);

        if (cinemasQuery is null)
        {
            throw new NotFoundException(DisplayTextFor.Cinema, request.CityId);
        }

        foreach (var cinemas in cinemasQuery)
        {
            var cinema = new GetCinemasForUser_Cinema
            {
                Id = cinemas.Id,
                Name = cinemas.Name,
            };

            foreach (var studiosQuery in cinemas.Studios)
            {
                var studios = new GetCinemasForUser_Studios
                {
                    Id = studiosQuery.Id,
                    Name = studiosQuery.Name
                };

                if (studiosQuery.Shows is not null)
                {
                    foreach (var shows in studiosQuery.Shows)
                    {
                        var show = new GetCinemasForUser_Shows
                        {
                            Id = shows.Id,
                            DateShow = shows.ShowDateTime.ToShortDateString(),
                            TimeShow = shows.ShowDateTime.ToShortTimeString(),
                        };

                        var movies = await _context.Movies
                            .Where(x => !x.IsDeleted && x.Id == shows.MovieId)
                            .Include(a => a.Shows)
                            .SingleOrDefaultAsync(cancellationToken);

                        if (movies is not null)
                        {
                            studios.MovieName = movies.Title;
                        }

                        studios.TicketPrice = shows.TicketPrice;

                        studios.Shows.Add(show);
                    }
                }

                cinema.Studios.Add(studios);
            }

            response.Items.Add(cinema);
        }

        return response;
    }
}
