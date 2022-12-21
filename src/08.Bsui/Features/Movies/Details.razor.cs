using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Components;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.Movies.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Movies.Constants;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Movies.Queries.GetMovie;

namespace Zeta.NontonFilm.Bsui.Features.Movies;

public partial class Details
{
    [Parameter]
    public Guid MovieId { get; set; }
    private string _genre = default!;
    private readonly List<string> _genreName = new();
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private GetMovieResponse? _movie;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _movieService.GetMovieAsync(MovieId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _movie = responseResult.Result;
            foreach (var genre in _movie.MovieGenres)
            {
                _genreName.Add(genre.GenreName);
            }

            _genre = string.Join(", ", _genreName);

            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_movie.Title));
        }

    }

    private async Task ShowDialogDelete()
    {
        if (_movie is null)
        {
            return;
        }

        var dialogTitle = $"{CommonDisplayTextFor.Delete} {DisplayTextFor.Movie} {_movie.Id}";

        var dialog = _dialogService.Show<DialogDelete>(dialogTitle);
        var result = await dialog.Result;

        if (!result.Cancelled)
        {
            await _movieService.DeleteMovieAsync(MovieId);

            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Delete.ToLower()} {DisplayTextFor.Movie} {_movie.Id}", Severity.Success);

            _navigationManager.NavigateTo(RouteFor.Index);
        }
    }
}
