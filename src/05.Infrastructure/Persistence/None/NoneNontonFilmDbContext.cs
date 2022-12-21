using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Infrastructure.Persistence.None;

public partial class NoneNontonFilmDbContext : DbContext
{
    private readonly ILogger<NoneNontonFilmDbContext> _logger;

    public NoneNontonFilmDbContext(ILogger<NoneNontonFilmDbContext> logger)
    {
        _logger = logger;
    }

    private void LogWarning()
    {
        _logger.LogWarning("{ServiceName} is set to None.", $"{nameof(Persistence)} {CommonDisplayTextFor.Service}");
    }

    public Task<int> SaveChangesAsync<THandler>(THandler handler, CancellationToken cancellationToken) where THandler : notnull
    {
        LogWarning();

        return Task.FromResult(0);
    }
}
