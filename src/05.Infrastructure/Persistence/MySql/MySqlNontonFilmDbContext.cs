using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.DateAndTime;
using Zeta.NontonFilm.Application.Services.DomainEvent;
using Zeta.NontonFilm.Infrastructure.Persistence.Common.Extensions;
using Zeta.NontonFilm.Infrastructure.Persistence.MySql.Configuration;

namespace Zeta.NontonFilm.Infrastructure.Persistence.MySql;

public class MySqlNontonFilmDbContext : NontonFilmDbContext
{
    public MySqlNontonFilmDbContext(
        DbContextOptions<MySqlNontonFilmDbContext> options,
        ICurrentUserService currentUser,
        IDateAndTimeService dateTime,
        IDomainEventService domainEvent) : base(options)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
        _domainEvent = domainEvent;
    }

    public MySqlNontonFilmDbContext(DbContextOptions<MySqlNontonFilmDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromNameSpace(typeof(AuditConfiguration).Namespace!);

        base.OnModelCreating(builder);
    }
}
