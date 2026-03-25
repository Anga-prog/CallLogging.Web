using CallLogging.Services.DTOs;

namespace CallLogging.Web.Models
{
    public class ClientDashboardViewModel
    {
        public IEnumerable<TicketListDto> Tickets { get; set; } = [];
        public ClientDashboardStatsDto Stats { get; set; } = new();
    }
}
