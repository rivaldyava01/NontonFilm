using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer.Configuration;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable(nameof(INontonFilmDbContext.Tickets), nameof(NontonFilm));
        builder.ConfigureCreatableProperties();

        builder.HasOne(e => e.Show).WithMany(e => e.Tickets).HasForeignKey(e => e.ShowId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.Seat).WithMany(e => e.Tickets).HasForeignKey(e => e.SeatId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.TicketSales).WithMany(e => e.Tickets).HasForeignKey(e => e.TicketSalesId).OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.TicketPrice).HasColumnType(ColumnTypes.Money);
    }
}
