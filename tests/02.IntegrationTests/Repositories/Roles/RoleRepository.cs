using Newtonsoft.Json;
using Zeta.NontonFilm.IntegrationTests.Repositories.Roles.Models;

namespace Zeta.NontonFilm.IntegrationTests.Repositories.Roles;

public static class RoleRepository
{
    private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, nameof(Repositories), nameof(Roles), "roles.json");

    public static List<Role> Roles => JsonConvert.DeserializeObject<List<Role>>(File.ReadAllText(_filePath))!;
}
