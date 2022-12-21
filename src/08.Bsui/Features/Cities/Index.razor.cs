using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Bsui.Features.Cities.Components;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cities.Commands.UpdateCity;
using Zeta.NontonFilm.Shared.Cities.Constants;
using Zeta.NontonFilm.Shared.Cities.Queries.GetCities;
using Zeta.NontonFilm.Shared.Cities.Queries.GetMovies;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cities;

public partial class Index
{
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private MudTable<GetCities_City> _tableCities = new();
    private string? _keyword;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(DisplayTextFor.Cities));
    }

    private async Task<TableData<GetCities_City>> ReloadTableCities(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetCities_City>();

        var request = state.ToPaginatedListRequest<GetCitiesRequest>(_keyword);

        var response = await _cityService.GetCitiesAsync(request);

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

        await _tableCities.ReloadServerData();
    }

    private async Task ShowDialogDelete(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.City} {id}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var responseResult = await _cityService.DeleteCityAsync(id);

            if (responseResult.Error is not null)
            {
                _error = responseResult.Error;
                return;
            }

            else
            {
                _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.City} {id}", Severity.Success);

                await _tableCities.ReloadServerData();
            }
        }
    }

    private async Task ShowDialogEdit(Guid id, string name)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Edit} {DisplayTextFor.City} {name}";

        var request = new UpdateCityRequest
        {
            Id = id,
            Name = name,
        };

        var dialogParameters = new DialogParameters
        {
            { nameof(DialogEdit.Request), request }
        };

        var dialog = _dialogService.Show<DialogEdit>(dialogTitle, dialogParameters);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Update.ToLower()} {DisplayTextFor.City} {name}", Severity.Success);
        }

    }

    private async Task ShowDialogAdd()
    {
        var dialogTitle = $"{CommonDisplayTextFor.Add} {DisplayTextFor.City}";

        var dialog = _dialogService.Show<DialogAdd>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var cityId = (Guid)result.Data;

            await _tableCities.ReloadServerData();
        }
    }

}
