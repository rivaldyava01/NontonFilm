using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class ShowConfiguration : IEntityTypeConfiguration<Show>
{
    public void Configure(EntityTypeBuilder<Show> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Shows), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.HasOne(e => e.Movie).WithMany(e => e.Shows).HasForeignKey(e => e.MovieId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Studio).WithMany(e => e.Shows).HasForeignKey(e => e.StudioId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.TicketPrice).HasColumnType(ColumnTypes.Money);
    }
}
