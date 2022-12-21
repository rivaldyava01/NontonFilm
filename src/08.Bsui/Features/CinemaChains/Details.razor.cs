using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Bsui.Features.CinemaChains.Components;
using Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.CinemaChains.Constants;
using Zeta.NontonFilm.Shared.CinemaChains.Queries.GetCinemaChain;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemas;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains;

public partial class Details
{
    [Parameter]
    public Guid CinemaChainId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private GetCinemaChainResponse? _cinemaChain;
    private MudTable<GetCinemas_Cinema> _tableCinemas = new();
    private string? _keyword;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _cinemaChainService.GetCinemaChainAsync(CinemaChainId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _cinemaChain = responseResult.Result;
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_cinemaChain.Name));
        }

    }

    private async Task ShowDialogDelete()
    {
        if (_cinemaChain is null)
        {
            return;
        }

        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.CinemaChain} {_cinemaChain.Name}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var responseResult = await _cinemaChainService.DeleteCinemaChainAsync(CinemaChainId);

            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;
                return;
            }
            else
            {
                _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.CinemaChain} {_cinemaChain.Id}", Severity.Success);

                _navigationManager.NavigateTo(RouteFor.Index);
            }
        }
    }

    private async Task<TableData<GetCinemas_Cinema>> ReloadTableCinemas(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetCinemas_Cinema>();

        var request = state.ToPaginatedListRequest<GetCinemasRequest>(_keyword);
        request.CinemaChainId = CinemaChainId;

        var response = await _cinemaService.GetCinemasAsync(request);

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

        await _tableCinemas.ReloadServerData();
    }

    private async Task ShowDialogAddCinema(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Add} {DisplayTextFor.Cinema}";

        var request = id;
        var dialogParameters = new DialogParameters
        {
            { nameof(DialogAddCinema.CinemaChainId), request }
        };

        var dialog = _dialogService.Show<DialogAddCinema>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var cinema = (Guid)result.Data;

            await _tableCinemas.ReloadServerData();
        }
    }
}
