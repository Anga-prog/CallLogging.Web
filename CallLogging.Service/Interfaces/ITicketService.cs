using CallLogging.Services.DTOs;

namespace CallLogging.Services.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketListDto>> GetClientTicketsAsync(int clientProfileID);
        Task<TicketListDto> GetTicketForClient(int ticketId, int clientProfileID);


        Task<IEnumerable<TicketListDto>> GetTicketListDtosAsync();
        Task<TicketListDto?> GetTicketForStaffAsync(int ticketId);
        Task UpdateTicketAsync(int ticketId, TicketUpdateDto dto);
        Task DeleteTicketAsync(int ticketId);
    }

}
