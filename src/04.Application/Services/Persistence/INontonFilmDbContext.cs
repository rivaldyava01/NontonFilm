using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Domain.Entities;

namespace Zeta.NontonFilm.Application.Services.Persistence;

public interface INontonFilmDbContext
{
    #region Essential Entities
    DbSet<Audit> Audits { get; }
    #endregion Essential Entities

    #region Business Entities
    DbSet<Cinema> Cinemas { get; }
    DbSet<CinemaChain> CinemaChains { get; }
    DbSet<City> Cities { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Movie> Movies { get; }
    DbSet<MovieGenre> MovieGenres { get; }
    DbSet<Seat> Seats { get; }
    DbSet<Show> Shows { get; }
    DbSet<Studio> Studios { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<TicketSales> TicketSales { get; }
    #endregion Business Entities

    Task<int> SaveChangesAsync<THandler>(THandler handler, CancellationToken cancellationToken = default) where THandler : notnull;
}
