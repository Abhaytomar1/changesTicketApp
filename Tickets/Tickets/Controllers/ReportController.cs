using Microsoft.AspNetCore.Mvc;
using System.Text;
using Tickets.Models; // Assuming you have a service to handle report generation

namespace Tickets.Controllers
{
    public class ReportController : Controller
    {
        private readonly ITicketService _ticketService;

        public ReportController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // Display the report generation form
        public IActionResult GenerateReport()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GenerateReport(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                return View();
            }

            var tickets = _ticketService.GetTicketsByDateRange(startDate, endDate);
            ViewData["StartDate"] = startDate.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate.ToString("yyyy-MM-dd");

            return View("GenerateReport", tickets);
        }

        public IActionResult ExportReport(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
            {
                return BadRequest("End date must be after start date.");
            }

            var tickets = _ticketService.GetTicketsByDateRange(startDate, endDate);
            var csv = GenerateCsv(tickets);
            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", "tickets_report.csv");
        }


        private string GenerateCsv(IEnumerable<Ticket> tickets)
        {
            var csv = new StringBuilder();
            csv.AppendLine("Id,Subject,TicketBody,CreatedAt,Priority,Status,AssignedUserId,AssignedByEmail");

            foreach (var ticket in tickets)
            {
                var id = EscapeCsvField(ticket.Id.ToString());
                //var ticketNo = EscapeCsvField(ticket.TicketNo);
                var subject = EscapeCsvField(ticket.Subject);
                var body = EscapeCsvField(ticket.TicketBody);
                var createdAt = EscapeCsvField(ticket.CreatedAt.ToString("yyyy-MM-dd"));
                var priority = EscapeCsvField(ticket.Priority.ToString());
                var status = EscapeCsvField(ticket.Status.ToString());
                var assignedUserId = EscapeCsvField(ticket.AssignedUserId.ToString());
                var assignedByEmail = EscapeCsvField(ticket.AssignedByEmail.ToString());

                csv.AppendLine($"{id},{subject},{body},{createdAt},{priority},{status},{assignedUserId},{assignedByEmail}");
            }

            return csv.ToString();
        }

        private string EscapeCsvField(string field)
        {
            if (field.Contains("\""))
            {
                field = field.Replace("\"", "\"\"");
            }
            if (field.Contains(",") || field.Contains("\n") || field.Contains("\""))
            {
                field = $"\"{field}\"";
            }
            return field;
        }
    }
}
