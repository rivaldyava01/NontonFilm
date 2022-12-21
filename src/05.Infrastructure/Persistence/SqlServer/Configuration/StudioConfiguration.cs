using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Shared.Studios.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class StudioConfiguration : IEntityTypeConfiguration<Studio>
{
    public void Configure(EntityTypeBuilder<Studio> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Studios), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.HasOne(e => e.Cinema).WithMany(e => e.Studios).HasForeignKey(e => e.CinemaId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Name).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Name));
    }
}
