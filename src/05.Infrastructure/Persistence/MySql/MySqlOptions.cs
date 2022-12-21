namespace Zeta.NontonFilm.Infrastructure.Persistence.MySql;

public class MySqlOptions
{
    public static readonly string SectionKey = $"{nameof(Persistence)}:{nameof(MySql)}";

    public string ConnectionString { get; set; } = default!;
}
