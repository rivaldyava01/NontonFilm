using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Shared.Seats.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Seats), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.HasOne(e => e.Studio).WithMany(e => e.Seats).HasForeignKey(e => e.StudioId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Row).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Row));
        builder.Property(e => e.Column).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Column));
    }
}
