using System.Net;

namespace Zeta.NontonFilm.Client.Common.Responses;

public class ErrorResponse
{
    public string Type { get; set; } = default!;
    public string Title { get; set; } = default!;
    public HttpStatusCode Status { get; set; }
    public string Detail { get; set; } = default!;
    public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
}
