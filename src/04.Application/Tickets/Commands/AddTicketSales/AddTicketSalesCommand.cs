using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Zeta.NontonFilm.Application.Common.Attributes;
using Zeta.NontonFilm.Application.Common.Exceptions;
using Zeta.NontonFilm.Application.Services.CurrentUser;
using Zeta.NontonFilm.Application.Services.Persistence;
using Zeta.NontonFilm.Domain.Entities;
using Zeta.NontonFilm.Shared.Common.Responses;
using Zeta.NontonFilm.Shared.Services.Authorization.Constants;
using Zeta.NontonFilm.Shared.Tickets.Commands.AddTicketSales;

namespace Zeta.NontonFilm.Application.Tickets.Commands.AddTicketSales;

[Authorize(Policy = Permissions.NontonFilm_Ticket_User_Handle)]

public class AddTicketSalesCommand : AddTicketSalesRequest, IRequest<ItemCreatedResponse>
{
}

public class AddTicketSalesCommandValidator : AbstractValidator<AddTicketSalesCommand>
{
    public AddTicketSalesCommandValidator()
    {
        Include(new AddTicketSalesRequestValidator());
    }
}

public class AddTicketSalesCommandHandler : IRequestHandler<AddTicketSalesCommand, ItemCreatedResponse>
{
    private readonly INontonFilmDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AddTicketSalesCommandHandler(INontonFilmDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<ItemCreatedResponse> Handle(AddTicketSalesCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
        {
            throw new NotFoundException();
        }

        var ticketSales = new TicketSales
        {
            Id = Guid.NewGuid(),
            UserId = (Guid)_currentUserService.UserId,
            SalesDateTime = DateTime.Now,
        };

        foreach (var ticketRequest in request.SeatCode)
        {
            var tickets = await _context.Tickets
                .Where(x => x.SeatCode == ticketRequest && x.ShowId == request.ShowId)
                .SingleOrDefaultAsync(cancellationToken);

            if (tickets is null)
            {
                throw new NotFoundException();
            }

            tickets.TicketSalesId = ticketSales.Id;
        }

        await _context.TicketSales.AddAsync(ticketSales, cancellationToken);
        await _context.SaveChangesAsync(this, cancellationToken);

        return new ItemCreatedResponse
        {
            Id = ticketSales.Id
        };
    }
}
