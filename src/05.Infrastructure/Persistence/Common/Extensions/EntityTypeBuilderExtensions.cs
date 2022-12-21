using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Domain.Events;
using Zeta.NontonFilm.Domain.Interfaces;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Constants;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void ConfigureCreatableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, ICreatable
    {
        builder.Property(e => e.CreatedBy).HasColumnType(CommonColumnTypes.Nvarchar(CommonMaximumLengthFor.CreatedBy));
    }

    public static void ConfigureModifiableProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IModifiable
    {
        builder.Property(x => x.Modified);
        builder.Property(e => e.ModifiedBy).HasColumnType(CommonColumnTypes.Nvarchar(CommonMaximumLengthFor.ModifiedBy));
    }

    public static void ConfigureFileProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasFile
    {
        builder.Property(e => e.FileName).HasColumnType(CommonColumnTypes.Nvarchar(CommonMaximumLengthFor.FileName));
        builder.Property(e => e.FileContentType).HasColumnType(CommonColumnTypes.Nvarchar(CommonMaximumLengthFor.FileContentType));
        builder.Property(e => e.StorageFileId).HasColumnType(CommonColumnTypes.Nvarchar(CommonMaximumLengthFor.StorageFileId));
    }

    public static void IgnoreDomainEventsProperties<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IHasDomainEvent
    {
        builder.Ignore(e => e.DomainEvents);
    }
}
