using Microsoft.AspNetCore.Mvc.Rendering;

namespace CallLogging.Web.Models
{
    public class ClientCreateTicketViewModel
    {
        public int ProductId { get; set; }
        public int CallTypeId { get; set; }
        public int PriorityId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Products { get; set; } = [];
        public IEnumerable<SelectListItem> CallTypes { get; set; } = [];
        public IEnumerable<SelectListItem> Priorities { get; set; } = [];
    }
}
