using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;

namespace Zeta.NontonFilm.Infrastructure.Persistence;

public partial class NontonFilmDbContext : INontonFilmDbContext
{
    #region Essential Entities
    public DbSet<Audit> Audits => Set<Audit>();
    #endregion Essential Entities

    #region Business Entities
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<MovieGenre> MovieGenres => Set<MovieGenre>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Cinema> Cinemas => Set<Cinema>();
    public DbSet<CinemaChain> CinemaChains => Set<CinemaChain>();
    public DbSet<Studio> Studios => Set<Studio>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Show> Shows => Set<Show>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<TicketSales> TicketSales => Set<TicketSales>();
    #endregion Business Entities
}
