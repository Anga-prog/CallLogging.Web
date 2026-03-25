using CallLogging.Services.DTOs;

namespace CallLogging.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<TicketListDto>> GetStaffDashboardTicketsAsync();
        Task<IEnumerable<AgentWorkloadDto>> GetAgentWorkloadsAsync();
        Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync();
        Task<DashboardStatsDto> GetDashboardStatsAsync();


        Task<IEnumerable<TicketListDto>> GetClientDashboardTicketsAsync(int clientProfileId);
        Task<ClientDashboardStatsDto> GetClientStatsAsync(int clientProfileId);
    }

}
