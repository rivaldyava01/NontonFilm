using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
{
    public void Configure(EntityTypeBuilder<Cinema> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Cinemas), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.HasOne(e => e.CinemaChain).WithMany(e => e.Cinemas).HasForeignKey(e => e.CinemaChainId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.City).WithMany(e => e.Cinemas).HasForeignKey(e => e.CityId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Name).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Name));
        builder.Property(e => e.EmailAddress).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.EmailAddress));
        builder.Property(e => e.Address).HasColumnType(ColumnTypes.NvarcharMax);
        builder.Property(e => e.PhoneNumber).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.PhoneNumber));
    }
}
