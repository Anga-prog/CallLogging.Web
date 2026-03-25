using CallLogging.Services.DTOs;

namespace CallLogging.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketListDto>> GetClientTicketsAsync(int clientProfileId);
        Task<TicketDetailDto?> GetTicketDetailForClientAsync(int ticketId, int clientProfileId);

        Task<IEnumerable<TicketListDto>> GetTicketListDtosAsync();
        Task<TicketDetailDto?> GetTicketDetailAsync(int ticketId);
        Task UpdateTicketAsync(int ticketId, TicketUpdateDto dto);
        Task AddLogUpdateAsync(LogUpdateCreateDto dto, int staffSystemUserId);
        Task<int> CreateTicketAsync(TicketCreateDto dto, int createdBySystemUserId);
        Task DeleteTicketAsync(int ticketId);

        Task<IEnumerable<LookupDto>> GetStatusesAsync();
        Task<IEnumerable<LookupDto>> GetPrioritiesAsync();
        Task<IEnumerable<LookupDto>> GetCallTypesAsync();
        Task<IEnumerable<LookupDto>> GetClientProductsAsync(int clientProfileId);
        Task<IEnumerable<AgentWorkloadDto>> GetAssignableAgentsAsync();
    }
}
