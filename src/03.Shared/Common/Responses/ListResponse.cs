namespace Zeta.NontonFilm.Shared.Common.Responses;

public class ListResponse<T>
{
    public IList<T> Items { get; set; } = new List<T>();
}
