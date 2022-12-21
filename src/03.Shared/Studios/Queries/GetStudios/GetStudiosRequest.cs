using Zeta.NontonFilm.Shared.Common.Requests;

namespace Zeta.NontonFilm.Shared.Studios.Queries.GetStudios;

public class GetStudiosRequest : PaginatedListRequest
{
    public Guid CinemaId { get; set; }
}
