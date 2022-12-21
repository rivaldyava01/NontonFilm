using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Common.Extensions;
using Zeta.NontonFilm.Bsui.Features.Genres.Components;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Genres.Commands.UpdateGenre;
using Zeta.NontonFilm.Shared.Genres.Constants;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenres;

namespace Zeta.NontonFilm.Bsui.Features.Genres;

public partial class Index
{
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();
    private MudTable<GetGenres_Genre> _tableGenres = new();
    private string? _keyword;
    private ErrorResponse? _error;
    private readonly GetGenreResponse _genre = new();

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(CommonBreadcrumbFor.Active(DisplayTextFor.Genres));
    }

    private async Task ShowDialogAdd()
    {
        var dialogTitle = $"{CommonDisplayTextFor.Add} {DisplayTextFor.Genre}";

        var dialog = _dialogService.Show<DialogAdd>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            var genreId = (Guid)result.Data;

            await _tableGenres.ReloadServerData();
        }
    }

    private async Task<TableData<GetGenres_Genre>> ReloadTableGenres(TableState state)
    {
        _error = null;

        StateHasChanged();
        var tableData = new TableData<GetGenres_Genre>();

        var request = state.ToPaginatedListRequest<GetGenresRequest>(_keyword);

        var response = await _genreService.GetGenresAsync(request);

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

        await _tableGenres.ReloadServerData();
    }

    private async Task ShowDialogDelete(Guid id)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.Genre} {id}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await _genreService.DeleteGenreAsync(id);

            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.Genre} {id}", Severity.Success);

            await _tableGenres.ReloadServerData();
        }
    }

    private async Task ShowDialogEdit(Guid id, string name)
    {
        var dialogTitle = $"{CommonDisplayTextFor.Edit} {DisplayTextFor.Genre} {name}";

        var request = new UpdateGenreRequest
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
            await _tableGenres.ReloadServerData();
        }

    }

}

