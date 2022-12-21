using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class CinemaChainConfiguration : IEntityTypeConfiguration<CinemaChain>
{
    public void Configure(EntityTypeBuilder<CinemaChain> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.CinemaChains), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Name).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Name));
        builder.Property(e => e.EmailAddress).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.EmailAddress));
        builder.Property(e => e.OfficeAddress).HasColumnType(ColumnTypes.NvarcharMax);
        builder.Property(e => e.PhoneNumber).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.PhoneNumber));
    }
}
