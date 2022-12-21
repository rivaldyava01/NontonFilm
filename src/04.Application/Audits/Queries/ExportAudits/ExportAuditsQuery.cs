using System.Globalization;
using CsvHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Services.DateAndTime;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Shared.Audits.Queries.ExportAudits;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;

namespace Zeta.NontonFilm.Application.Audits.Queries.ExportAudits;

[Authorize(Policy = Permissions.NontonFilm_Audit_Index)]
public class ExportAuditsQuery : ExportAuditsRequest, IRequest<ExportAuditsResponse>
{
}

public class ExportAuditsQueryHandler : IRequestHandler<ExportAuditsQuery, ExportAuditsResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly IDateAndTimeService _dateTime;

    public ExportAuditsQueryHandler(INontonFilmDbContext context, IDateAndTimeService dateTime)
    {
        _context = context;
        _dateTime = dateTime;
    }

    public async Task<ExportAuditsResponse> Handle(ExportAuditsQuery request, CancellationToken cancellationToken)
    {
        var audits = await _context.Audits
                .Where(x => request.AuditIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        csvWriter.WriteRecords(audits);

        var content = memoryStream.ToArray();

        return new ExportAuditsResponse
        {
            ContentType = CommonContentTypes.TextCsv,
            Content = content,
            FileName = $"Audits_{audits.Count}_{_dateTime.Now:yyyyMMdd_HHmmss}.csv"
        };
    }
}
