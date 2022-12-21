using MudBlazor;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.Bsui.Common.Extensions;

public static class PaginatedListResponseExtensions
{
    public static TableData<T> ToTableData<T>(this PaginatedListResponse<T> result)
    {
        return new TableData<T>() { TotalItems = result.TotalCount, Items = result.Items };
    }
}
