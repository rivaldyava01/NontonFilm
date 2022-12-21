using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Common.Extensions;

namespace Zeta.NontonFilm.Bsui.Features.MemberArea.Cinemas;

public partial class Index
{
    private ErrorResponse? _error;
    private bool _showpanel = true;
    private readonly GetCitiesForUser _citiesUser = new();
    private readonly List<GetCitiesForUser_City> _cities = new();
    private readonly List<GetCinemasForUser_cinemas> _cinemas = new();

    protected override async Task OnInitializedAsync()
    {
        await ReloadCity();
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
            _cities.Add(new GetCitiesForUser_City
            {
                Id = city.Id,
                Name = city.Name
            });
        }
    }

    private Task<IEnumerable<GetCitiesForUser_City>> SearchCity(string keyword)
    {
        var result = _cities.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _cities.Where(x => x.Name.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    private async Task ShowCinemas(Guid id)
    {
        _showpanel = false;
        _cinemas.Clear();

        var cinemaResponse = await _cinemaService.GetCinemaForUserAsync(id);
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
            var cinemas = new GetCinemasForUser_cinemas
            {
                Id = cinema.Id,
                Name = cinema.Name,
            };
            foreach (var studioquery in cinema.Studios)
            {
                var studios = new GetCinemasForUser_Studios
                {
                    Id = studioquery.Id,
                    Name = studioquery.Name,
                    MovieName = studioquery.MovieName,
                    TicketPrice = studioquery.TicketPrice.ToCurrency0DisplayText()

                };

                foreach (var show in studioquery.Shows)
                {
                    var shows = new GetCinemasForUser_Shows
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

public class GetCitiesForUser
{
    public GetCitiesForUser_City City { get; set; } = new();
}

public class GetCitiesForUser_City
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
}

public class GetCinemasForUser_cinemas
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;

    public List<GetCinemasForUser_Studios> Studios { get; set; } = new();
}

public class GetCinemasForUser_Studios
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string MovieName { get; set; } = default!;
    public string TicketPrice { get; set; } = default!;

    public List<GetCinemasForUser_Shows> Shows { get; set; } = new();
}

public class GetCinemasForUser_Shows
{
    public Guid Id { get; set; } = default!;
    public string DateShow { get; set; } = default!;
    public string TimeShow { get; set; } = default!;

}
