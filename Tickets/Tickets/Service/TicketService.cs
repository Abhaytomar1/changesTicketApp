using System.Collections.Generic;
using System.Linq;
using Tickets.Models;
using YourNamespace.Data;

public class TicketService : ITicketService
{
    private readonly ApplicationDbContext _context;

    public TicketService(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Ticket> GetAllTickets()
    {
        return _context.Tickets.ToList();
    }
    public IEnumerable<Ticket> GetTicketsByDateRange(DateTime startDate, DateTime endDate)
    {
        return _context.Tickets
            .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
            .ToList();
    }
}
