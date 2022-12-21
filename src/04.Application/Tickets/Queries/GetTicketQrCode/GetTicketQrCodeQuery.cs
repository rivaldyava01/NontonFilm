using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Application.Services.QrCode;
using Zeta.NontonFilm.Shared.Common.Constants;
using Zeta.NontonFilm.Shared.Tickets.Constants;
using Zeta.NontonFilm.Shared.Tickets.Queries.GetTicketQrCode;

namespace Zeta.NontonFilm.Application.Tickets.Queries.GetTicketQrCode;

public class GetTicketQrCodeQuery : GetTicketQrCodeRequest, IRequest<GetTicketQrCodeResponse>
{
}

public class GetTicketQrCodeQueryHandler : IRequestHandler<GetTicketQrCodeQuery, GetTicketQrCodeResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IQrCodeService _qrCodeService;

    public GetTicketQrCodeQueryHandler(INontonFilmDbContext context, ICurrentUserService currentUserService, IQrCodeService qrCodeService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _qrCodeService = qrCodeService;
    }

    public async Task<GetTicketQrCodeResponse> Handle(GetTicketQrCodeQuery request, CancellationToken cancellationToken)
    {
        var ticketSales = await _context.TicketSales
            .Where(x => x.UserId == _currentUserService.UserId && x.Id == request.TicketSalesId)
            .SingleOrDefaultAsync(cancellationToken);

        if (ticketSales is null)
        {
            throw new NotFoundException(DisplayTextFor.TicketSales, request.TicketSalesId);
        }

        return new GetTicketQrCodeResponse
        {
            Data = $"data:{CommonContentTypes.ImagePng};base64,{Convert.ToBase64String(_qrCodeService.GetGraphic(request.TicketSalesId.ToString()))}"
        };
    }

}

