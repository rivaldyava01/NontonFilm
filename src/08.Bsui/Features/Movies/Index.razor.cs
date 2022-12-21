using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovies;

namespace Zeta.NontonFilm.Bsui.Features.Movies;

public partial class Index
{
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private MudTable<GetMovies_Movie> _tableMovies = new();
    private string? _keyword;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(DisplayTextFor.Movies));
    }

    private async Task<TableData<GetMovies_Movie>> ReloadTableMovies(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetMovies_Movie>();

        var request = state.ToPaginatedListRequest<GetMoviesRequest>(_keyword);

        var response = await _movieService.GetMoviesAsync(request);

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
        _keyword = keyword;

        await _tableMovies.ReloadServerData();
    }

}
