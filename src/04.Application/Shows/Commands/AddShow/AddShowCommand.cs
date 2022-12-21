using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Shows.Commands.AddShow;
using Zeta.NontonFilm.Shared.Shows.Constants;

namespace Zeta.NontonFilm.Application.Shows.Commands.AddShow;

[Authorize(Policy = Permissions.NontonFilm_Show_Add)]

public class AddShowCommand : AddShowRequest, IRequest<ItemCreatedResponse>
{
}

public class AddShowCommandValidator : AbstractValidator<AddShowCommand>
{
    public AddShowCommandValidator()
    {
        Include(new AddShowRequestValidator());
    }
}

public class AddShowCommandHandler : IRequestHandler<AddShowCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;

    public AddShowCommandHandler(INontonFilmDbContext context)
    {
        _context = context;
    }

    public async Task<ItemCreatedResponse> Handle(AddShowCommand request, CancellationToken cancellationToken)
    {
        var studio = await _context.Studios
            .Where(x => !x.IsDeleted && x.Id == request.StudioId)
            .Include(a => a.Seats)
            .SingleOrDefaultAsync(cancellationToken);

        if (studio == null)
        {
            throw new NotFoundException(nameof(studio), request.StudioId);
        }

        var movie = await _context.Movies
          .Where(x => !x.IsDeleted && x.Id == request.MovieId)
          .SingleOrDefaultAsync(cancellationToken);

        if (movie == null)
        {
            throw new NotFoundException(nameof(movie), request.MovieId);
        }

        var cinema = await _context.Cinemas
            .Where(x => !x.IsDeleted && x.Id == studio.CinemaId)
            .SingleOrDefaultAsync(cancellationToken);

        if (cinema == null)
        {
            throw new NotFoundException(nameof(cinema), studio.CinemaId);
        }

        var showWithTheSameData = await _context.Shows
            .Where(x => !x.IsDeleted
                        && x.StudioId == request.StudioId
                        && x.ShowDateTime == request.ShowDateTime)
            .SingleOrDefaultAsync(cancellationToken);

        if (showWithTheSameData is not null)
        {
            throw new AlreadyExistsException(DisplayTextFor.Show, DisplayTextFor.DateTime, request.ShowDateTime);
        }

        var showWithTheSameMovie = await _context.Shows
          .Where(x => !x.IsDeleted
                      && x.StudioId == request.StudioId
                      && x.ShowDateTime.Date == request.ShowDateTime.Date && x.MovieId != request.MovieId)
          .SingleOrDefaultAsync(cancellationToken);

        if (showWithTheSameData is not null)
        {
            throw new OneMovieOneDaysException(DisplayTextFor.Show, DisplayTextFor.Movie, movie.Title);
        }

        if (request.ShowDateTime < DateTime.Now)
        {
            throw new EarlyDateException(nameof(Show), request.ShowDateTime, nameof(DateTime.Now));
        }

        if (request.ShowDateTime < movie.ReleaseDate)
        {
            throw new EarlyDateException(nameof(Show), request.ShowDateTime, nameof(movie.ReleaseDate));
        }

        var show = new Show
        {
            Id = Guid.NewGuid(),
            MovieId = movie.Id,
            StudioId = studio.Id,
            TicketPrice = request.TicketPrice,
            ShowDateTime = request.ShowDateTime
        };

        foreach (var seat in studio.Seats)
        {
            var ticket = new Ticket
            {
                Id = Guid.NewGuid(),
                ShowId = show.Id,
                SeatId = seat.Id,
                TicketPrice = show.TicketPrice,
                ShowDateTime = show.ShowDateTime,
                SeatCode = seat.Row + seat.Column,
                StudioName = studio.Name,
                MovieName = movie.Title,
                CinemaName = cinema.Name
            };
            await _context.Tickets.AddAsync(ticket, cancellationToken);
        }

        await _context.Shows.AddAsync(show, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = show.Id
        };
    }
}
