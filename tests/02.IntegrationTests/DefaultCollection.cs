using Zeta.NontonFilm.IntegrationTests.Application;
using Xunit;

namespace Zeta.NontonFilm.IntegrationTests;

[CollectionDefinition(nameof(ApplicationFixture), DisableParallelization = true)]
public class DefaultCollection : ICollectionFixture<ApplicationFixture>
{
}
