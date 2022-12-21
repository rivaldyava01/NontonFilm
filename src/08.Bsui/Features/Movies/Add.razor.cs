using Microsoft.AspNetCore.Components.Forms;
using Zeta.NontonFilm.Base.Enums;
using Zeta.NontonFilm.Bsui.Features.Movies.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Movies.Commands.AddMovie;

namespace Zeta.NontonFilm.Bsui.Features.Movies;

public partial class Add
{
    private ErrorResponse? _error;
    private readonly AddMovieCommand _request = new();

    private readonly List<AddMovieCommand_Genre> _genres = new();
    private readonly AddMovieCommand_MovieGenre _movieGenre = new();
    private IEnumerable<AddMovieCommand_Genre> Options { get; set; } = new HashSet<AddMovieCommand_Genre>();

    private DateTime? _date = DateTime.Today;
    private RatingTypes _rating = RatingTypes.SU;

    protected override async Task OnInitializedAsync()
    {

        var responseResult = await _genreService.GetListGenresAsync();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is null)
        {
            return;
        }

        foreach (var genre in responseResult.Result.Items)
        {
            _genres.Add(new AddMovieCommand_Genre
            {
                Id = genre.Id,
                Name = genre.Name
            });
        }
    }

    private async Task OnValidSubmit()
    {
        if (_request is null || _date is null)
        {
            return;
        }

        _error = null;

        _request.Rating = _rating;
        _request.ReleaseDate = _date.Value;

        foreach (var genre in Options)
        {
            _request.MovieGenres.Add(new AddMovieCommand_MovieGenre { Genre = genre });
        }

        var request = new AddMovieRequest
        {
            Title = _request.Title,
            Rating = _request.Rating,
            Duration = _request.Duration,
            ReleaseDate = _request.ReleaseDate,
            Synopsis = _request.Synopsis,
            PosterImage = _request.PosterImage,
        };

        foreach (var detail in _request.MovieGenres)
        {
            request.MovieGenreIds.Add(detail.Genre.Id);
        }

        var responseResult = await _movieService.AddMovieAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil menambahkan Movie. ID: {responseResult.Result.Id}", MudBlazor.Severity.Success);
            _navigationManager.NavigateTo(RouteFor.Index);
        }
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        foreach (var validationMessage in editContext.GetValidationMessages())
        {
            _snackbar.Add(validationMessage, MudBlazor.Severity.Error);
        }
    }
}

public class AddMovieCommand
{
    public string Title { get; set; } = default!;
    public RatingTypes Rating { get; set; }
    public int Duration { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; } = default!;
    public string PosterImage { get; set; } = default!;

    public List<AddMovieCommand_MovieGenre> MovieGenres { get; set; } = new();
}

public class AddMovieCommand_MovieGenre
{
    public AddMovieCommand_Genre Genre { get; set; } = default!;
}

public class AddMovieCommand_Genre
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public override string ToString()
    {
        return Name;
    }
}
