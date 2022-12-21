namespace Zeta.NontonFilm.Shared.Audits.Queries.ExportAudits;

public class ExportAuditsRequest
{
    public IList<Guid> AuditIds { get; set; } = new List<Guid>();
}
