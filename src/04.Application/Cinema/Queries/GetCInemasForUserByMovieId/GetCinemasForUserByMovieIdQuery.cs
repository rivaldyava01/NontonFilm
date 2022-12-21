using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Cinemas.Queries.GetCinemasForUserByMovieId;

[Authorize(Policy = Permissions.NontonFilm_Movie_NowShowing)]

public class GetCinemasForUserByMovieIdQuery : GetCinemasForUserByMovieIdRequest, IRequest<ListResponse<GetCinemasForUserByMovieId_Cinema>>
{

}

public class GetCinemaForUserByMovieIdQueryHandler : IRequestHandler<GetCinemasForUserByMovieIdQuery, ListResponse<GetCinemasForUserByMovieId_Cinema>>
{
    private readonly INontonFilmDbContext _context;

    public GetCinemaForUserByMovieIdQueryHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ListResponse<GetCinemasForUserByMovieId_Cinema>> Handle(GetCinemasForUserByMovieIdQuery request, CancellationToken cancellationToken)
    {
        var response = new ListResponse<GetCinemasForUserByMovieId_Cinema>();

        var movies = await _context.Movies
                    .Where(x => !x.IsDeleted && x.Id == request.MovieId)
                    .Include(a => a.Shows)
                    .SingleOrDefaultAsync(cancellationToken);

        if (movies is null)
        {
            throw new NotFoundException(DisplayTextFor.Movie, request.MovieId);
        }

        var cinemasQuery = await _context.Cinemas
            .AsNoTracking()
            .Where(x => !x.IsDeleted && x.CityId == request.CityId & x.Studios.Any())
            .Include(a => a.Studios.Where(x => x.Shows.Any()))
                .ThenInclude(b => b.Shows.Where(b => b.ShowDateTime > DateTime.Now && b.MovieId == movies.Id && !b.IsDeleted))
            .ToListAsync(cancellationToken);

        if (cinemasQuery is null)
        {
            throw new NotFoundException(DisplayTextFor.Cinema, request.CityId);
        }

        foreach (var cinemas in cinemasQuery)
        {
            var cinema = new GetCinemasForUserByMovieId_Cinema
            {
                Id = cinemas.Id,
                Name = cinemas.Name,
            };

            foreach (var studiosQuery in cinemas.Studios)
            {
                var studios = new GetCinemasForUserByMovieId_Studios
                {
                    Id = studiosQuery.Id,
                    Name = studiosQuery.Name
                };

                if (studiosQuery.Shows is not null)
                {
                    foreach (var shows in studiosQuery.Shows)
                    {
                        var show = new GetCinemasForUserByMovieId_Shows
                        {
                            Id = shows.Id,
                            DateShow = shows.ShowDateTime.ToShortDateString(),
                            TimeShow = shows.ShowDateTime.ToShortTimeString(),
                        };

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
