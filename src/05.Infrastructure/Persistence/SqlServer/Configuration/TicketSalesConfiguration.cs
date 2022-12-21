using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class TicketSalesConfiguration : IEntityTypeConfiguration<TicketSales>
{
    public void Configure(EntityTypeBuilder<TicketSales> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.TicketSales), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();
    }
}
