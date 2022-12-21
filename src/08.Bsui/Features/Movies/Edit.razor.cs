using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Base.Enums;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.Movies.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.MovieGenres.Commands.UpdateMovieGenre;
using Zeta.NontonFilm.Shared.Movies.Commands.UpdateMovie;

namespace Zeta.NontonFilm.Bsui.Features.Movies;

public partial class Edit
{
    [Parameter]
    public Guid MovieId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();
    private DateTime? _date = DateTime.Today;
    private RatingTypes _rating = RatingTypes.SU;

    private UpdateMovieCommand? _request;
    private readonly List<UpdateMovieCommand_Genre> _genres = new();
    private readonly UpdateMovieCommand_MovieGenre _movieGenre = new();
    private IEnumerable<UpdateMovieCommand_Genre> Options { get; set; } = new HashSet<UpdateMovieCommand_Genre>();
    private bool _isLoading;
    private ErrorResponse? _error;

    protected override void OnInitialized()
    {
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    protected override async Task OnParametersSetAsync()
    {
        await LoadDataGenres();

        _isLoading = true;

        var responseResult = await _movieService.GetMovieAsync(MovieId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            var movie = responseResult.Result;

            _request = new UpdateMovieCommand
            {
                Id = MovieId,
                Title = movie.Title,
                Rating = movie.Rating,
                Duration = movie.Duration,
                ReleaseDate = movie.ReleaseDate,
                Synopsis = movie.Synopsis,
                PosterImage = movie.PosterImage,
            };

            _date = _request.ReleaseDate;

            foreach (var genre in movie.MovieGenres)
            {
                foreach (var option in Options)
                {
                    option.Id = genre.Id;
                    option.Name = genre.GenreName;
                }
            }

            _breadcrumbItems.Add(BreadcrumbItemFor.Details(movie.Id, movie.Title));
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Edit));
        }

        _isLoading = false;
    }

    private async Task LoadDataGenres()
    {
        _isLoading = true;

        var responseResult = await _genreService.GetListGenresAsync();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is null)
        {
            return;
        }

        foreach (var genre in responseResult.Result.Items)
        {
            _genres.Add(new UpdateMovieCommand_Genre
            {
                Id = genre.Id,
                Name = genre.Name
            });
        }
    }

    private Task<IEnumerable<UpdateMovieCommand_Genre>> SearchGenre(string keyword)
    {
        var result = _genres.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _genres.Where(x => x.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    private async Task OnValidSubmit()
    {
        if (_request is null || _date is null)
        {
            return;
        }

        _isLoading = true;

        _error = null;

        _request.Rating = _rating;
        _request.ReleaseDate = _date.Value;

        foreach (var genre in Options)
        {
            _request.MovieGenres.Add(new UpdateMovieCommand_MovieGenre { Genre = genre });
        }

        var request = new UpdateMovieRequest
        {
            Id = _request.Id,
            Title = _request.Title,
            Rating = _request.Rating,
            Duration = _request.Duration,
            ReleaseDate = _request.ReleaseDate,
            Synopsis = _request.Synopsis,
            PosterImage = _request.PosterImage,
        };

        foreach (var detail in _request.MovieGenres)
        {
            request.MovieGenres.Add(new UpdateMovieRequest_MovieGenre
            {
                GenreId = detail.Genre.Id,
            });
        }

        var responseResult = await _movieService.UpdateMovieAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        _isLoading = false;

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil update Movie. ID: {MovieId}", Severity.Success);

            _navigationManager.NavigateTo(RouteFor.Details(MovieId));
        }
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        foreach (var validationMessage in editContext.GetValidationMessages())
        {
            _snackbar.Add(validationMessage, Severity.Error);
        }
    }
}

public class UpdateMovieCommand
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<UpdateMovieCommand_MovieGenre> MovieGenres { get; set; } = new();
}

public class UpdateMovieCommand_MovieGenre
{
    public UpdateMovieCommand_Genre Genre { get; set; } = default!;
}

public class UpdateMovieCommand_Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public override string ToString()
    {
        return Name;
    }

}
