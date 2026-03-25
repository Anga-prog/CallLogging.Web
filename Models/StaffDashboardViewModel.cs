using CallLogging.Services.DTOs;

namespace CallLogging.Web.Models
{
    public class StaffDashboardViewModel
    {
        public IEnumerable<TicketListDto> Tickets { get; set; } = [];
        public DashboardStatsDto Stats { get; set; } = new();
        public IEnumerable<AgentWorkloadDto> AgentWorkloads { get; set; } = [];
        public IEnumerable<RecentActivityDto> RecentActivities { get; set; } = [];
    }
}
