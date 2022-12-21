using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Constants;
using Zeta.NontonFilm.Shared.Movies.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Movies), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
        builder.ConfigureModifiableProperties();

        builder.Property(e => e.Title).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.Title));
        builder.Property(e => e.Synopsis).HasColumnType(ColumnTypes.NvarcharMax);
    }
}
