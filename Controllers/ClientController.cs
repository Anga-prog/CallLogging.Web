using System.Security.Claims;
using CallLogging.Services.DTOs;
using CallLogging.Services.Interfaces;
using CallLogging.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallLogging.Web.Controllers
{
    [Authorize(Policy = "ClientOnly")]
    public class ClientController : Controller
    {
        private readonly IDashboardService _dashboard;
        private readonly ITicketService _tickets;

        public ClientController(IDashboardService dashboard, ITicketService tickets)
        {
            _dashboard = dashboard;
            _tickets = tickets;
        }

        private int ClientProfileId =>
            int.TryParse(User.FindFirstValue("ClientProfileId"), out var id) ? id : 0;

        public async Task<IActionResult> Dashboard()
        {
            var vm = new ClientDashboardViewModel
            {
                Tickets = await _dashboard.GetClientDashboardTicketsAsync(ClientProfileId),
                Stats = await _dashboard.GetClientStatsAsync(ClientProfileId)
            };
            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var ticket = await _tickets.GetTicketDetailForClientAsync(id, ClientProfileId);
            if (ticket == null) return NotFound();
            return View(ticket);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await BuildCreateVmAsync();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientCreateTicketViewModel form)
        {
            if (!ModelState.IsValid)
            {
                var vm = await BuildCreateVmAsync();
                vm.ProductId = form.ProductId;
                vm.CallTypeId = form.CallTypeId;
                vm.PriorityId = form.PriorityId;
                vm.Title = form.Title;
                vm.Description = form.Description;
                return View(vm);
            }

            var ticketId = await _tickets.CreateTicketAsync(new TicketCreateDto
            {
                ClientProfileId = ClientProfileId,
                ProductId = form.ProductId,
                CallTypeId = form.CallTypeId,
                Title = form.Title,
                Description = form.Description,
                PriorityId = form.PriorityId
            }, createdBySystemUserId: 0);

            return RedirectToAction(nameof(Detail), new { id = ticketId });
        }

        private async Task<ClientCreateTicketViewModel> BuildCreateVmAsync()
        {
            var products = await _tickets.GetClientProductsAsync(ClientProfileId);
            var callTypes = await _tickets.GetCallTypesAsync();
            var priorities = await _tickets.GetPrioritiesAsync();

            return new ClientCreateTicketViewModel
            {
                Products = products.Select(p => new SelectListItem(p.Name, p.Id.ToString())),
                CallTypes = callTypes.Select(c => new SelectListItem(c.Name, c.Id.ToString())),
                Priorities = priorities.Select(p => new SelectListItem(p.Name, p.Id.ToString()))
            };
        }
    }
}
