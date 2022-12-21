using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Shows.Commands.AddShow;
using Zeta.NontonFilm.Shared.Shows.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Studios.Components;

public partial class DialogAddShow
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid StudioId { get; set; }

    private readonly AddShowCommand _request = new();
    private readonly List<AddShowCommand_Movie> _movies = new();

    private TimeSpan? _time = new TimeSpan();
    private DateTime? _date = DateTime.Today;
    private ErrorResponse? _error;

    protected override async Task OnInitializedAsync()
    {
        await ReloadMovie();
    }

    private async Task ReloadMovie()
    {
        var responseResult = await _movieService.GetListMoviesAsync();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is null)
        {
            return;
        }

        foreach (var movie in responseResult.Result.Items)
        {
            _movies.Add(new AddShowCommand_Movie
            {
                Id = movie.Id,
                Title = movie.Title
            });
        }
    }

    private Task<IEnumerable<AddShowCommand_Movie>> SearchMovie(string keyword)
    {
        var result = _movies.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _movies.Where(x => x.Title.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    private void Cancel()
    {
        MudDialog.Cancel();
    }

    private async Task OnValidSubmit()
    {

        if (_date is null || _time is null)
        {
            return;
        }

        if (_request.Movie is null)
        {
            _snackbar.Add($"Movie Cannot be empty", Severity.Error);
            return;
        }

        var showDate = _date.Value + _time.Value;
        _error = null;

        var request = new AddShowRequest
        {
            MovieId = _request.Movie.Id,
            StudioId = StudioId,
            ShowDateTime = showDate,
            TicketPrice = _request.TicketPrice
        };

        var responseResult = await _showService.AddShowAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Succesfully {CommonDisplayTextFor.Add.ToLower()} {DisplayTextFor.Show} {responseResult.Result.Id}", Severity.Success);

            MudDialog.Close(DialogResult.Ok(responseResult.Result.Id));
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

public class AddShowCommand
{
    public AddShowCommand_Movie Movie { get; set; } = default!;
    public Guid StudioId { get; set; }
    public DateTime ShowDateTime { get; set; }
    public decimal TicketPrice { get; set; }
}

public class AddShowCommand_Movie
{

    public Guid Id { get; set; }
    public string Title { get; set; } = default!;

}

