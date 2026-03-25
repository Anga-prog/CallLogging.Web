using System.Security.Claims;
using CallLogging.Services.DTOs;
using CallLogging.Services.Interfaces;
using CallLogging.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallLogging.Web.Controllers
{
    [Authorize(Policy = "StaffOnly")]
    public class StaffController : Controller
    {
        private readonly IDashboardService _dashboard;
        private readonly ITicketService _tickets;

        public StaffController(IDashboardService dashboard, ITicketService tickets)
        {
            _dashboard = dashboard;
            _tickets = tickets;
        }

        public async Task<IActionResult> Dashboard()
        {
            var vm = new StaffDashboardViewModel
            {
                Tickets = await _dashboard.GetStaffDashboardTicketsAsync(),
                Stats = await _dashboard.GetDashboardStatsAsync(),
                AgentWorkloads = await _dashboard.GetAgentWorkloadsAsync(),
                RecentActivities = await _dashboard.GetRecentActivitiesAsync()
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var ticket = await _tickets.GetTicketDetailAsync(id);
            if (ticket == null) return NotFound();

            var statuses = await _tickets.GetStatusesAsync();
            var priorities = await _tickets.GetPrioritiesAsync();
            var agents = await _tickets.GetAssignableAgentsAsync();

            var vm = new StaffTicketDetailViewModel
            {
                Ticket = ticket,
                Statuses = statuses.Select(s => new SelectListItem(s.Name, s.Id.ToString())),
                Priorities = priorities.Select(p => new SelectListItem(p.Name, p.Id.ToString())),
                Agents = new[] { new SelectListItem("— Unassigned —", "0") }
                    .Concat(agents.Select(a => new SelectListItem(a.FullName, a.SystemUserId.ToString()))),
                NewStatusId = ticket.StatusId,
                NewPriorityId = ticket.PriorityId,
                NewAssignedToId = ticket.AssignedToId
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, StaffTicketDetailViewModel form)
        {
            var systemUserIdClaim = User.FindFirstValue("SystemUserId");
            if (!int.TryParse(systemUserIdClaim, out int systemUserId))
                return Forbid();

            // Update ticket metadata
            await _tickets.UpdateTicketAsync(id, new TicketUpdateDto
            {
                StatusId = form.NewStatusId,
                PriorityId = form.NewPriorityId,
                AssignedToId = form.NewAssignedToId == 0 ? null : form.NewAssignedToId
            });

            // Add log entry if description was provided
            if (!string.IsNullOrWhiteSpace(form.UpdateDescription))
            {
                await _tickets.AddLogUpdateAsync(new LogUpdateCreateDto
                {
                    CallLogId = id,
                    Description = form.UpdateDescription,
                    StatusId = form.NewStatusId,
                    TimeSpentMinutes = form.TimeSpentMinutes
                }, systemUserId);
            }

            return RedirectToAction(nameof(Detail), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _tickets.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
