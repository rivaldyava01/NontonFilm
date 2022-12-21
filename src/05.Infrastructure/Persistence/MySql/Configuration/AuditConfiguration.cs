using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Base.ValueObjects;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.MySql.Constants;
using Zeta.NontonFilm.Shared.Audits.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.MySql.Configuration;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Audits));
        builder.ConfigureCreatableProperties();

        builder.Property(e => e.TableName).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.TableName));
        builder.Property(e => e.EntityName).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.EntityName));
        builder.Property(e => e.ActionType).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.ActionType));
        builder.Property(e => e.ActionName).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.ActionName));
        builder.Property(e => e.OldValues).HasColumnType(ColumnTypes.LongText);
        builder.Property(e => e.NewValues).HasColumnType(ColumnTypes.LongText);
        builder.Property(e => e.ClientApplicationId).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.ClientApplicationId));
        builder.Property(e => e.FromIpAddress).HasColumnType(CommonColumnTypes.Nvarchar(MaximumLengthFor.FromIpAddress));

        builder.OwnsOne(o => o.FromGeolocation, p =>
        {
            p.Property(p => p.Latitude).HasColumnName(nameof(Geolocation.Latitude));
            p.Property(p => p.Longitude).HasColumnName(nameof(Geolocation.Longitude));
            p.Property(p => p.Accuracy).HasColumnName(nameof(Geolocation.Accuracy));
        });
    }
}
