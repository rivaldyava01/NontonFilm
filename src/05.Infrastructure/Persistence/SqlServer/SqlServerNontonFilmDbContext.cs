using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.DateAndTime;
using Zeta.NontonFilm.Application.Services.DomainEvent;

namespace Zeta.NontonFilm.Infrastructure.Persistence.SqlServer;

public class SqlServerNontonFilmDbContext : NontonFilmDbContext
{
    public SqlServerNontonFilmDbContext(
        DbContextOptions<SqlServerNontonFilmDbContext> options,
        ICurrentUserService currentUser,
        IDateAndTimeService dateTime,
        IDomainEventService domainEvent) : base(options)
    {
        _currentUser = currentUser;
        _dateTime = dateTime;
        _domainEvent = domainEvent;
    }

    public SqlServerNontonFilmDbContext(DbContextOptions<SqlServerNontonFilmDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
