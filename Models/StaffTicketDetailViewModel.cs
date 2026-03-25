using CallLogging.Services.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallLogging.Web.Models
{
    public class StaffTicketDetailViewModel
    {
        public TicketDetailDto Ticket { get; set; } = null!;
        public IEnumerable<SelectListItem> Statuses { get; set; } = [];
        public IEnumerable<SelectListItem> Priorities { get; set; } = [];
        public IEnumerable<SelectListItem> Agents { get; set; } = [];

        // Update form fields
        public int NewStatusId { get; set; }
        public int NewPriorityId { get; set; }
        public int? NewAssignedToId { get; set; }
        public string UpdateDescription { get; set; } = string.Empty;
        public int? TimeSpentMinutes { get; set; }
    }
}
