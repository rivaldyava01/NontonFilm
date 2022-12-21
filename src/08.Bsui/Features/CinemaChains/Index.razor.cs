using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChains;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains;

public partial class Index
{
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private MudTable<GetCinemaChains_CinemaChain> _tableCinemaChains = new();
    private string? _keyword;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(DisplayTextFor.CinemaChains));
    }

    private async Task<TableData<GetCinemaChains_CinemaChain>> ReloadTableCinemaChains(TableState state)
    {
        _error = null;

        StateHasChanged();

        var tableData = new TableData<GetCinemaChains_CinemaChain>();

        var request = state.ToPaginatedListRequest<GetCinemaChainsRequest>(_keyword);

        var response = await _cinemaChainService.GetCinemaChainsAsync(request);

        _error = response.Error;

        StateHasChanged();

        if (response.Result is null)
        {
            return tableData;
        }

        return response.Result.ToTableData();
    }

    private async Task OnSearch(string keyword)
    {
        _keyword = keyword.Trim();

        await _tableCinemaChains.ReloadServerData();
    }

}
