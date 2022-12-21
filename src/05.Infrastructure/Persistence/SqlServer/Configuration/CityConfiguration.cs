using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Shared.Cities.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Cities), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Name).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Name));
    }
}
