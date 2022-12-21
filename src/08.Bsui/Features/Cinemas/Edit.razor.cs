using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Zeta.NontonFilm.Bsui.Common.Constants;
using Zeta.NontonFilm.Bsui.Features.Cinemas.Constants;
using Zeta.NontonFilm.Client.Common.Responses;
using Zeta.NontonFilm.Shared.Cinemas.Commands.UpdateCinema;
using Zeta.NontonFilm.Shared.Common.Constants;

namespace Zeta.NontonFilm.Bsui.Features.Cinemas;

public partial class Edit
{
    [Parameter]
    public Guid CinemaId { get; set; }

    private readonly List<BreadcrumbItem> _breadcrumbItems = new();
    private bool _isLoading;
    private ErrorResponse? _error;
    private UpdateCinemaCommand _request = new();
    private readonly List<UpdateCinemaCommand_City> _cities = new();
    private readonly List<UpdateCinemaCommand_CinemaChain> _cinemaChains = new();

    protected override async Task OnParametersSetAsync()
    {
        await ReloadCity();
        await ReloadCinemaChain();

        _isLoading = true;

        var responseResult = await _cinemaService.GetCinemaAsync(CinemaId);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;

            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            var cinema = responseResult.Result;

            _request = new UpdateCinemaCommand
            {
                Id = CinemaId,
                Name = cinema.Name,
                Address = cinema.Address,
                EmailAddress = cinema.EmailAddress,
                PhoneNumber = cinema.PhoneNumber,
                City = _cities.Single(x => x.CityId == cinema.CityId),
                CinemaChain = _cinemaChains.Single(x => x.CinemaChainId == cinema.CinemaChainId),
            };

            _breadcrumbItems.Add(BreadcrumbItemFor.Details(cinema.Id, cinema.Name));
            _breadcrumbItems.Add(CommonBreadcrumbFor.Active(CommonDisplayTextFor.Edit));
        }

        _isLoading = false;
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
            _cities.Add(new UpdateCinemaCommand_City
            {
                CityId = city.Id,
                CityName = city.Name
            });
        }
    }

    private async Task ReloadCinemaChain()
    {
        var responseResult = await _cinemaChainService.GetListCinemaChainsAsync();

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
            _cinemaChains.Add(new UpdateCinemaCommand_CinemaChain
            {
                CinemaChainId = city.Id,
                CinemaChainName = city.Name
            });
        }
    }

    private Task<IEnumerable<UpdateCinemaCommand_CinemaChain>> SearchCinemaChain(string keyword)
    {
        var result = _cinemaChains.AsEnumerable();

        if (!string.IsNullOrEmpty(keyword))
        {
            result = _cinemaChains.Where(x => x.CinemaChainName.Contains(keyword, StringComparison.InvariantCultureIgnoreCase));
        }

        return Task.FromResult(result);
    }

    private Task<IEnumerable<UpdateCinemaCommand_City>> SearchCity(string keyword)
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
        _isLoading = true;

        _error = null;
        var request = new UpdateCinemaRequest
        {
            Id = CinemaId,
            Name = _request.Name,
            Address = _request.Address,
            EmailAddress = _request.EmailAddress,
            PhoneNumber = _request.PhoneNumber,
            CinemaChainId = _request.CinemaChain.CinemaChainId,
            CityId = _request.City.CityId,
        };

        var responseResult = await _cinemaService.UpdateCinemaAsync(request);

        if (responseResult.Error is not null)
        {
            _error = responseResult.Error;
            _isLoading = false;

            return;
        }

        if (responseResult.Result is not null)
        {
            _snackbar.Add($"Success Update Cinema. ID: {CinemaId}", Severity.Success);
            _navigationManager.NavigateTo(RouteFor.Details(CinemaId));
        }

        _isLoading = false;
    }

    private void OnInvalidSubmit(EditContext editContext)
    {
        foreach (var validationMessage in editContext.GetValidationMessages())
        {
            _snackbar.Add(validationMessage, Severity.Error);
        }
    }
}

public class UpdateCinemaCommand
{
    public Guid Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;

    public UpdateCinemaCommand_City City { get; set; } = default!;
    public UpdateCinemaCommand_CinemaChain CinemaChain { get; set; } = default!;
}

public class UpdateCinemaCommand_City
{
    public Guid CityId { get; set; } = default!;
    public string CityName { get; set; } = default!;
}

public class UpdateCinemaCommand_CinemaChain
{
    public Guid CinemaChainId { get; set; } = default!;
    public string CinemaChainName { get; set; } = default!;
}
