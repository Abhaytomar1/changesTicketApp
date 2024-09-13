using System.Collections.Generic;
using Tickets.Models;

public interface ITicketService
{
    IEnumerable<Ticket> GetAllTickets();
    IEnumerable<Ticket> GetTicketsByDateRange(DateTime startDate, DateTime endDate);

}
