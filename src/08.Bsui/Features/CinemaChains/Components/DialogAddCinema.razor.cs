using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Features.CinemaChains.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cinemas.Commands.AddCinema;

namespace Zeta.NontonFilm.Bsui.Features.CinemaChains.Components;

public partial class DialogAddCinema
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = default!;

    [Parameter]
    public Guid CinemaChainId { get; set; }

    private ErrorResponse? _error;
    private readonly AddCinemaCommand _request = new();
    private readonly List<AddCinemaCommand_City> _cities = new();

    protected override async Task OnInitializedAsync()
    {
        await ReloadCity();
    }

    private async Task ReloadCity()
    {
        var responseResult = await _cityService.GetListCitiesAsync();

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
            _cities.Add(new AddCinemaCommand_City
            {
                CityId = city.Id,
                CityName = city.Name
            });
        }
    }

    private Task<IEnumerable<AddCinemaCommand_City>> SearchCity(string keyword)
    {
        var result = _cities.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _cities.Where(x => x.CityName.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    private async Task OnValidSubmit()
    {
        _error = null;

        if (_request.City is null)
        {
            _snackbar.Add($"City ", Severity.Error);
            return;
        }

        var request = new AddCinemaRequest
        {
            Name = _request.Name,
            Address = _request.Address,
            EmailAddress = _request.EmailAddress,
            PhoneNumber = _request.PhoneNumber,
            CinemaChainId = CinemaChainId,
            CityId = _request.City.CityId,
        };

        var responseResult = await _cinemaService.AddCinemaAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            return;
        }

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Berhasil menambahkan Cinema. ID: {responseResult.Result.Id}", Severity.Success);
            _navigationManager.NavigateTo(RouteFor.Index);
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

public class AddCinemaCommand
{
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public Guid CinemaChainId { get; set; } = default!;

    public AddCinemaCommand_City City { get; set; } = default!;
}

public class AddCinemaCommand_City
{
    public Guid CityId { get; set; } = default!;
    public string CityName { get; set; } = default!;
}
