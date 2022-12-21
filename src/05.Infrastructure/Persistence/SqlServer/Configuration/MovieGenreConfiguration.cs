using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{
    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.MovieGenres), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();

        builder.HasOne(e => e.Genre).WithMany(e => e.MovieGenres).HasForeignKey(e => e.GenreId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Movie).WithMany(e => e.MovieGenres).HasForeignKey(e => e.MovieId).OnDelete(DeleteBehavior.Restrict);
    }
}
