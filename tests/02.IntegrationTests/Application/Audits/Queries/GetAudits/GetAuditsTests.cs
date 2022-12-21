﻿using FluentAssertions;
using Xunit;
using Zeta.NontonFilm.Application.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.IntegrationTests.Repositories.Constants;
using Zeta.NontonFilm.Shared.Audits.Queries.GetAudits;
using Zeta.NontonFilm.Shared.Common.Enums;

namespace Zeta.NontonFilm.IntegrationTests.Application.Audits.Queries.GetAudits;

[Collection(nameof(ApplicationFixture))]
[Trait(nameof(Domain), nameof(Audit))]
public class GetAuditsTests
{
    private readonly ApplicationFixture _fixture;

    public GetAuditsTests(ApplicationFixture fixture)
    {
        _fixture = fixture;
        _fixture.ResetState().Wait();
    }

    [Fact]
    public async Task Should_Get_Audits()
    {
        _fixture.RunAsUser(UsernameFor.TicketingMultiRole, PositionIdFor.KepalaTeknologiInformasi);

        var query = new GetAuditsQuery
        {
            Page = 1,
            PageSize = 10,
            SearchText = null,
            SortField = nameof(GetAuditsAudit.Created),
            SortOrder = SortOrder.Desc
        };

        var result = await _fixture.SendAsync(query);

        result.Items.Count.Should().BeGreaterOrEqualTo(0);
    }
}
