using Microsoft.AspNetCore.Mvc;
using Zeta.NontonFilm.Shared.Common.Responses;

namespace Zeta.NontonFilm.WebApi.Common.Extensions;

public static class FileResponseExtensions
{
    public static FileContentResult AsFile(this FileResponse fileResponse)
    {
        return new FileContentResult(fileResponse.Content, fileResponse.ContentType)
        {
            FileDownloadName = fileResponse.FileName
        };
    }
}
