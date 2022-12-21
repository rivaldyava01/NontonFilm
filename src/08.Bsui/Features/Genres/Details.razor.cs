using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.Genres.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Genres.Queries.GetGenre;

namespace Zeta.NontonFilm.Bsui.Features.Genres;

public partial class Details
{
    [Parameter]
    public Guid GenreId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private GetGenreResponse _genre = new();

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _genreService.GetGenreAsync(GenreId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _genre = responseResult.Result;
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(_genre.Name.ToString()));
        }

    }
}
