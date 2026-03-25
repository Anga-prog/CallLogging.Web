using CallLogging.Data.Models;
using CallLogging.Services.DTOs;
using CallLogging.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CallLogging.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly CallLoggingContext _context;

        public DashboardService(CallLoggingContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TicketListDto>> GetStaffDashboardTicketsAsync()
        {
            return await _context.OpenTicketsDashboards
                .AsNoTracking()
                .OrderBy(t => t.PrioritySortOrder)
                .ThenBy(t => t.CreatedAt)
                .Select(t => new TicketListDto
                {
                    Id = t.TicketId,
                    Title = t.Title,
                    CallType = t.CallType,
                    Priority = t.Priority,
                    PrioritySortOrder = t.PrioritySortOrder,
                    Status = t.Status,
                    IsFinal = false,
                    Product = t.Product,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    AssignedTo = t.AssignedTo,
                    ClientCompany = t.ClientCompany,
                    ClientName = t.ClientName,
                    AgeHours = t.AgeHours ?? 0,
                    IsAwaitingClient = t.IsAwaitingClient == 1
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AgentWorkloadDto>> GetAgentWorkloadsAsync()
        {
            return await _context.AgentWorkloads
                .AsNoTracking()
                .Select(w => new AgentWorkloadDto
                {
                    SystemUserId = w.SystemUserId,
                    FullName = w.FullName,
                    Email = w.Email,
                    Role = w.Role,
                    TotalAssigned = w.TotalAssigned ?? 0,
                    OpenTickets = w.OpenTickets ?? 0,
                    AwaitingClient = w.AwaitingClient ?? 0,
                    HighPlusTickets = w.HighPlusTickets ?? 0,
                    WorkloadLevel = w.WorkloadLevel
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<RecentActivityDto>> GetRecentActivitiesAsync()
        {
            return await _context.RecentActivities
                .AsNoTracking()
                .Select(a => new RecentActivityDto
                {
                    SystemUserId = a.SystemUserId,
                    FullName = a.FullName,
                    TicketId = a.TicketId,
                    TicketTitle = a.TicketTitle,
                    StatusAfterUpdate = a.StatusAfterUpdate,
                    UpdateDescription = a.UpdateDescription,
                    TimeSpentMinutes = a.TimeSpentMinutes,
                    ActivityAt = a.ActivityAt,
                    MinutesAgo = a.MinutesAgo ?? 0
                })
                .ToListAsync();
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var tickets = await _context.OpenTicketsDashboards.AsNoTracking().ToListAsync();

            return new DashboardStatsDto
            {
                TotalOpen = tickets.Count,
                Unassigned = tickets.Count(t => t.AssignedToId == null),
                OpenByPriority = tickets
                    .GroupBy(t => t.Priority)
                    .ToDictionary(g => g.Key, g => g.Count())
            };
        }

        public async Task<IEnumerable<TicketListDto>> GetClientDashboardTicketsAsync(int clientProfileId)
        {
            return await _context.MyTickets
                .AsNoTracking()
                .Where(t => t.ClientProfileId == clientProfileId)
                .OrderBy(t => t.PrioritySortOrder)
                .ThenByDescending(t => t.CreatedAt)
                .Select(t => new TicketListDto
                {
                    Id = t.TicketId,
                    Title = t.Title,
                    CallType = t.CallType,
                    Priority = t.Priority,
                    PrioritySortOrder = t.PrioritySortOrder,
                    Status = t.Status,
                    IsFinal = t.IsFinal,
                    Product = t.Product,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    AssignedTo = t.AssignedTo,
                    UpdateCount = t.UpdateCount ?? 0,
                    LastUpdatedAt = t.LastUpdatedAt
                })
                .ToListAsync();
        }

        public async Task<ClientDashboardStatsDto> GetClientStatsAsync(int clientProfileId)
        {
            var tickets = await _context.MyTickets
                .AsNoTracking()
                .Where(t => t.ClientProfileId == clientProfileId)
                .ToListAsync();

            return new ClientDashboardStatsDto
            {
                TotalTickets = tickets.Count,
                OpenTickets = tickets.Count(t => !t.IsFinal),
                ResolvedTickets = tickets.Count(t => t.IsFinal)
            };
        }
    }
}
