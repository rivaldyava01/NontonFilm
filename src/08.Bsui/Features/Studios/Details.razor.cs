using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Bsui.Features.Studios.Components;
using Zeta.NontonFilm.Bsui.Features.Studios.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Shows.Queries.GetPastShows;
using Zeta.NontonFilm.Shared.Shows.Queries.GetUpcomingShows;
using Zeta.NontonFilm.Shared.Studios.Constants;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudio;

namespace Zeta.NontonFilm.Bsui.Features.Studios;

public partial class Details
{
    [Parameter]
    public Guid StudioId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetPastShows_Show> _tablePastShows = new();
    private MudTable<GetUpcomingShows_Show> _tableUpcomingShows = new();
    private string? _keyword;

    private ErrorResponse? _error;
    private GetStudioResponse _studio = new();

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.CinemaIndex);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _studioService.GetStudioAsync(StudioId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _studio = responseResult.Result;
            var cinemaresponseResult = await _cinemaService.GetCinemaAsync(_studio.CinemaId);

            if (cinemaresponseResult.Error is not null)
            {
                _error = cinemaresponseResult.Error;

                return;
            }

            if (cinemaresponseResult.Result is not null)
            {
                _breadcrumbItems.Add(BreadcrumbItemFor.CinemaChainDetails(cinemaresponseResult.Result.CinemaChainId, cinemaresponseResult.Result.CinemaChainName));
                _breadcrumbItems.Add(BreadcrumbItemFor.CinemaDetails(_studio.CinemaId, _studio.CinemaName));
                _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_studio.Name.ToString()));
            }

        }

    }

    private async Task<TableData<GetPastShows_Show>> ReloadTablePastShows(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetPastShows_Show>();

        var request = state.ToPaginatedListRequest<GetPastShowsRequest>(_keyword);
        request.StudioId = StudioId;

        var response = await _showService.GetPastShowsAsync(request);

        _error = response.Error;

        StateHasChanged();

        if (response.Result is null)
        {
            return tableData;
        }

        return response.Result.ToTableData();
    }

    private async Task OnPastSearch(string keyword)
    {
        _keyword = keyword;

        await _tablePastShows.ReloadServerData();
    }

    private async Task<TableData<GetUpcomingShows_Show>> ReloadTableUpcomingShows(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetUpcomingShows_Show>();

        var request = state.ToPaginatedListRequest<GetUpcomingShowsRequest>(_keyword);
        request.StudioId = StudioId;

        var response = await _showService.GetUpcomingShowsAsync(request);

        _error = response.Error;

        StateHasChanged();

        if (response.Result is null)
        {
            return tableData;
        }

        return response.Result.ToTableData();
    }

    private async Task OnUpcomingSearch(string keyword)
    {
        _keyword = keyword;

        await _tableUpcomingShows.ReloadServerData();
    }

    private async Task ShowDialogAddShow(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Add} {DisplayTextFor.Show}";

        var request = id;
        var dialogParameters = new DialogParameters
        {
            { nameof(DialogAddShow.StudioId), request }
        };

        var dialog = _dialogService.Show<DialogAddShow>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var studioId = (Guid)result.Data;

            await _tableUpcomingShows.ReloadServerData();
        }
    }

    private async Task ShowDialogDeleteUpcomingShow(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.Show} {id}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var responseResult = await _showService.DeleteShowAsync(id);
            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;
                return;
            }

            else
            {
                _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.Studios} {id}", Severity.Success);
                await _tableUpcomingShows.ReloadServerData();
            }
        }
    }
}
