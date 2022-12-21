using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.MemberArea.Movies.Constants;
using Zeta.NontonFilm.Client.Common.Responses;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Movies;

public partial class NowShowing
{
    private readonly List<BreadcrumbItem> _breadcrumbItems = new();

    private ErrorResponse? _error;
    private readonly List<GetNowShowingMovies> _movie = new();

    protected override async Task OnInitializedAsync()
    {
        var responseResult = await _movieService.GetNowShowingMoviesAsync();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            foreach (var movie in responseResult.Result.Items)
            {
                _movie.Add(new GetNowShowingMovies
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    PosterImage = movie.PosterImage
                });
            }
        }

        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }
}

public class GetNowShowingMovies
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string PosterImage { get; set; } = default!;
}
