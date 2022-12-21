using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;

namespace Zeta.NontonFilm.Infrastructure.Persistence.None;

public partial class NoneNontonFilmDbContext : INontonFilmDbContext
{
    #region Essential Entities
    public DbSet<Audit> Audits => Set<Audit>();
    #endregion Essential Entities

    #region Business Entities
    public DbSet<Genre> Genres => throw new NotImplementedException();

    public DbSet<MovieGenre> MovieGenres => throw new NotImplementedException();

    public DbSet<Movie> Movies => throw new NotImplementedException();

    public DbSet<City> Cities => throw new NotImplementedException();

    public DbSet<Cinema> Cinemas => throw new NotImplementedException();

    public DbSet<CinemaChain> CinemaChains => throw new NotImplementedException();

    public DbSet<Studio> Studios => throw new NotImplementedException();

    public DbSet<Seat> Seats => throw new NotImplementedException();

    public DbSet<Show> Shows => throw new NotImplementedException();

    public DbSet<Ticket> Tickets => throw new NotImplementedException();

    public DbSet<TicketSales> TicketSales => throw new NotImplementedException();
    #endregion Business Entities
}
