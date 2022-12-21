using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Bsui.Features.Cinemas.Components;
using Zeta.NontonFilm.Bsui.Features.Cinemas.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cinemas.Constants;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinema;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Studios.Commands.UpdateStudio;
using Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;

namespace Zeta.NontonFilm.Bsui.Features.Cinemas;

public partial class Details
{
    [Parameter]
    public Guid CinemaId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private GetCinemaResponse? _cinema;
    private MudTable<GetStudios_Studio> _tableStudios = new();
    private string? _keyword;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.CinemaIndex);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _cinemaService.GetCinemaAsync(CinemaId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _cinema = responseResult.Result;
            _breadcrumbItems.Add(BreadcrumbItemFor.CinemaChainDetails(_cinema.CinemaChainId, _cinema.CinemaChainName));
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_cinema.Name));
        }

    }

    private async Task ShowDialogDelete()
    {
        if (_cinema is null)
        {
            return;
        }

        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.Cinema} {_cinema.Name}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var responseResult = await _cinemaService.DeleteCinemaAsync(CinemaId);

            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;
                return;
            }
            else
            {
                _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.Cinema} {_cinema.Name}", Severity.Success);

                _navigationManager.NavigateTo(RouteFor.CinemaChainDetails(_cinema.CinemaChainId));
            }
        }
    }

    private async Task ShowDialogAddStudio(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Add} {DisplayTextFor.Studio}";

        var request = id;
        var dialogParameters = new DialogParameters
        {
            { nameof(DialogAddStudio.CinemaId), request }
        };

        var dialog = _dialogService.Show<DialogAddStudio>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var studioId = (Guid)result.Data;

            await _tableStudios.ReloadServerData();
        }
    }

    private async Task<TableData<GetStudios_Studio>> ReloadTableStudios(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetStudios_Studio>();

        var request = state.ToPaginatedListRequest<GetStudiosRequest>(_keyword);
        request.CinemaId = CinemaId;

        var response = await _studioService.GetStudiosAsync(request);

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

        await _tableStudios.ReloadServerData();
    }

    private async Task ShowDialogDeleteStudio(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.Studio} {id}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var responseResult = await _studioService.DeleteStudioAsync(id);

            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;
                return;
            }

            else
            {
                _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.Studios} {id}", Severity.Success);

                await _tableStudios.ReloadServerData();
            }
        }
    }

    private async Task ShowDialogEditStudio(Guid id, string name, Guid cinemaId)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Edit} {DisplayTextFor.Studio} {name}";

        var request = new UpdateStudioRequest
        {
            Id = id,
            Name = name,
            CinemaId = cinemaId
        };

        var dialogParameters = new DialogParameters
        {
            { nameof(DialogEditStudio.Request), request }
        };

        var dialog = _dialogService.Show<DialogEditStudio>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Update.ToLower()} {DisplayTextFor.Studio} {name}", Severity.Success);
            await _tableStudios.ReloadServerData();
        }

    }

}
