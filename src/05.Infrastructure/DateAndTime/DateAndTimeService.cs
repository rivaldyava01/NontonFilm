using Zeta.NontonFilm.Application.Services.DateAndTime;

namespace Zeta.NontonFilm.Infrastructure.DateAndTime;

public class DateAndTimeService : IDateAndTimeService
{
    public DateTimeOffset Now => DateTimeOffset.Now;
}
