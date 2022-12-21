using Microsoft.AspNetCore.Components;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.Movies.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cinemas.Queries.GetCinemasForUserByMovieId;
using Zeta.NontonFilm.Shared.Common.Extensions;
using Zeta.NontonFilm.Shared.Movies.Queries.GetNowShowingMovie;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Movies;

public partial class Details
{
    [Parameter]
    public Guid MovieId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();
    private string _genre = default!;
    private readonly List<string> _genreName = new();
    private readonly GetCitiesForUserByMovieId _citiesUser = new();
    private readonly List<GetCitiesForUserByMovieId_City> _cities = new();
    private readonly List<GetCinemasForUserByMovieId_cinemas> _cinemas = new();
    private bool _showpanel = true;

    private ErrorResponse? _error;
    private GetNowShowingMovieResponse? _movie;

    protected override async Task OnInitializedAsync()
    {
        await ReloadCity();
        _breadcrumbItems.Add(CommonBreadcrumbFor.Home);
        _breadcrumbItems.Add(BreadcrumbItemFor.Index);
    }

    private async Task ReloadCity()
    {
        var responseResult = await _cityService.GetCitiesForUserAsync();

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is null)
        {
            return;
        }

        foreach (var city in responseResult.Result.Items)
        {
            _cities.Add(new GetCitiesForUserByMovieId_City
            {
                Id = city.Id,
                Name = city.Name
            });
        }
    }

    private Task<IEnumerable<GetCitiesForUserByMovieId_City>> SearchCity(string keyword)
    {
        var result = _cities.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _cities.Where(x => x.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    protected override async Task OnParametersSetAsync()
    {

        var responseResult = await _movieService.GetNowShowingMovieAsync(MovieId);

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

    private async Task ShowCinemas(Guid cityId, Guid movieId)
    {
        _showpanel = false;
        _cinemas.Clear();

        var request = new GetCinemasForUserByMovieIdRequest
        {
            CityId = cityId,
            MovieId = movieId
        };

        var cinemaResponse = await _cinemaService.GetCinemaForUserByMovieIdAsync(request);
        if (cinemaResponse.Error is not null)
        {
            _error = cinemaResponse.Error;

            return;
        }

        if (cinemaResponse.Result is null)
        {
            return;
        }

        foreach (var cinema in cinemaResponse.Result.Items)
        {
            var cinemas = new GetCinemasForUserByMovieId_cinemas
            {
                Id = cinema.Id,
                Name = cinema.Name,
            };
            foreach (var studioquery in cinema.Studios)
            {
                var studios = new GetCinemasForUserByMovieId_Studios
                {
                    Id = studioquery.Id,
                    Name = studioquery.Name,
                    TicketPrice = studioquery.TicketPrice.ToCurrency0DisplayText()

                };

                foreach (var show in studioquery.Shows)
                {
                    var shows = new GetCinemasForUserByMovieId_Shows
                    {
                        Id = show.Id,
                        DateShow = show.DateShow,
                        TimeShow = show.TimeShow,
                    };

                    studios.Shows.Add(shows);
                }

                cinemas.Studios.Add(studios);
            }

            _cinemas.Add(cinemas);
        }
    }
}

public class GetCitiesForUserByMovieId
{
    public GetCitiesForUserByMovieId_City City { get; set; } = new();
}

public class GetCitiesForUserByMovieId_City
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class GetCinemasForUserByMovieId_cinemas
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public List<GetCinemasForUserByMovieId_Studios> Studios { get; set; } = new();
}

public class GetCinemasForUserByMovieId_Studios
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string TicketPrice { get; set; } = default!;

    public List<GetCinemasForUserByMovieId_Shows> Shows { get; set; } = new();
}

public class GetCinemasForUserByMovieId_Shows
{
    public Guid Id { get; set; } = default!;
    public string DateShow { get; set; } = default!;
    public string TimeShow { get; set; } = default!;

}
