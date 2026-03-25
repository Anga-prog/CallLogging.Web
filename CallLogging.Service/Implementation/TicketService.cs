using CallLogging.Data.Models;
using CallLogging.Services.DTOs;
using CallLogging.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CallLogging.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly CallLoggingContext _context;

        public TicketService(CallLoggingContext context)
        {
            _context = context;
        }

        // ── Client ──────────────────────────────────────────────────────────

        public async Task<IEnumerable<TicketListDto>> GetClientTicketsAsync(int clientProfileId)
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
                    LastUpdatedAt = t.LastUpdatedAt,
                    ClientProfileId = t.ClientProfileId,
                    ClientCompany = "",
                    ClientName = "",
                    AgeHours = 0,
                    IsAwaitingClient = false
                })
                .ToListAsync();
        }

        public async Task<TicketDetailDto?> GetTicketDetailForClientAsync(int ticketId, int clientProfileId)
        {
            var log = await GetLogWithIncludes()
                .FirstOrDefaultAsync(l => l.Id == ticketId && l.ClientProfileId == clientProfileId);

            return log == null ? null : MapToDetail(log);
        }

        // ── Staff ────────────────────────────────────────────────────────────

        public async Task<IEnumerable<TicketListDto>> GetTicketListDtosAsync()
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
                    UpdateCount = 0,
                    ClientCompany = t.ClientCompany,
                    ClientName = t.ClientName,
                    AgeHours = t.AgeHours ?? 0,
                    IsAwaitingClient = t.IsAwaitingClient == 1
                })
                .ToListAsync();
        }

        public async Task<TicketDetailDto?> GetTicketDetailAsync(int ticketId)
        {
            var log = await GetLogWithIncludes().FirstOrDefaultAsync(l => l.Id == ticketId);
            return log == null ? null : MapToDetail(log);
        }

        public async Task UpdateTicketAsync(int ticketId, TicketUpdateDto dto)
        {
            var log = await _context.Logs.FindAsync(ticketId);
            if (log == null) return;

            if (dto.Title != null) log.Title = dto.Title;
            if (dto.Description != null) log.Description = dto.Description;
            if (dto.PriorityId.HasValue) log.PriorityId = dto.PriorityId.Value;
            if (dto.StatusId.HasValue) log.StatusId = dto.StatusId.Value;
            if (dto.AssignedToId.HasValue) log.AssignedToId = dto.AssignedToId == 0 ? null : dto.AssignedToId;
            if (dto.RelatedToCallId.HasValue) log.RelatedToCallId = dto.RelatedToCallId;
            if (dto.MergedIntoCallId.HasValue) log.MergedIntoCallId = dto.MergedIntoCallId;
            log.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task AddLogUpdateAsync(LogUpdateCreateDto dto, int staffSystemUserId)
        {
            var update = new LogUpdate
            {
                CallLogId = dto.CallLogId,
                UpdatedById = staffSystemUserId,
                Description = dto.Description,
                StatusId = dto.StatusId,
                TimeSpentMinutes = dto.TimeSpentMinutes,
                HasAttachment = false,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.LogUpdates.Add(update);

            // Mirror status change onto the parent log
            if (dto.StatusId.HasValue)
            {
                var log = await _context.Logs.FindAsync(dto.CallLogId);
                if (log != null)
                {
                    log.StatusId = dto.StatusId.Value;
                    log.UpdatedAt = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateTicketAsync(TicketCreateDto dto, int createdBySystemUserId)
        {
            // If no system user is provided (client portal), fall back to the first available staff.
            if (createdBySystemUserId == 0)
                createdBySystemUserId = await _context.SystemUsers.Select(u => u.Id).FirstOrDefaultAsync();

            var log = new Log
            {
                ClientProfileId = dto.ClientProfileId,
                ProductId = dto.ProductId,
                CallTypeId = dto.CallTypeId,
                Title = dto.Title,
                Description = dto.Description,
                PriorityId = dto.PriorityId,
                StatusId = 1, // default Open
                CreatedById = createdBySystemUserId,
                AssignedToId = dto.AssignedToId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
            return log.Id;
        }

        public async Task DeleteTicketAsync(int ticketId)
        {
            var log = await _context.Logs.FindAsync(ticketId);
            if (log != null)
            {
                _context.Logs.Remove(log);
                await _context.SaveChangesAsync();
            }
        }

        // ── Lookups ──────────────────────────────────────────────────────────

        public async Task<IEnumerable<LookupDto>> GetStatusesAsync()
            => await _context.LogStatuses.AsNoTracking()
                .OrderBy(s => s.Id)
                .Select(s => new LookupDto { Id = s.Id, Name = s.StatusName })
                .ToListAsync();

        public async Task<IEnumerable<LookupDto>> GetPrioritiesAsync()
            => await _context.Priorities.AsNoTracking()
                .OrderBy(p => p.SortOrder)
                .Select(p => new LookupDto { Id = p.Id, Name = p.PriorityName })
                .ToListAsync();

        public async Task<IEnumerable<LookupDto>> GetCallTypesAsync()
            => await _context.CallTypes.AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new LookupDto { Id = c.Id, Name = c.Name })
                .ToListAsync();

        public async Task<IEnumerable<LookupDto>> GetClientProductsAsync(int clientProfileId)
            => await _context.ClientProducts.AsNoTracking()
                .Where(cp => cp.ClientProfileId == clientProfileId)
                .Select(cp => new LookupDto { Id = cp.Product.Id, Name = cp.Product.Name })
                .ToListAsync();

        public async Task<IEnumerable<AgentWorkloadDto>> GetAssignableAgentsAsync()
            => await _context.SystemUsers.AsNoTracking()
                .Select(u => new AgentWorkloadDto
                {
                    SystemUserId = u.Id,
                    FullName = u.Person.FullName,
                    Email = u.Person.Email,
                    Role = u.Role.Name,
                    TotalAssigned = 0,
                    OpenTickets = 0,
                    AwaitingClient = 0,
                    HighPlusTickets = 0,
                    WorkloadLevel = ""
                })
                .ToListAsync();

        // ── Helpers ──────────────────────────────────────────────────────────

        private IQueryable<Log> GetLogWithIncludes()
            => _context.Logs
                .Include(l => l.ClientProfile).ThenInclude(cp => cp.Person)
                .Include(l => l.Priority)
                .Include(l => l.Status)
                .Include(l => l.Product)
                .Include(l => l.CallType)
                .Include(l => l.AssignedTo!).ThenInclude(u => u.Person)
                .Include(l => l.LogUpdates).ThenInclude(u => u.Status)
                .Include(l => l.LogUpdates).ThenInclude(u => u.UpdatedBy).ThenInclude(u => u.Person);

        private static TicketDetailDto MapToDetail(Log log) => new()
        {
            Id = log.Id,
            Title = log.Title,
            Description = log.Description,
            Status = log.Status.StatusName,
            StatusId = log.StatusId,
            IsFinal = log.Status.IsFinal,
            Priority = log.Priority.PriorityName,
            PriorityId = log.PriorityId,
            ClientProfileId = log.ClientProfileId,
            Product = log.Product.Name,
            CallType = log.CallType.Name,
            ClientCompany = log.ClientProfile.Company ?? "",
            ClientName = log.ClientProfile.Person.FullName,
            AssignedTo = log.AssignedTo?.Person.FullName,
            AssignedToId = log.AssignedToId,
            CreatedAt = log.CreatedAt,
            UpdatedAt = log.UpdatedAt,
            Updates = log.LogUpdates
                .OrderBy(u => u.CreatedAt)
                .Select(u => new LogUpdateItemDto
                {
                    Id = u.Id,
                    Description = u.Description,
                    Status = u.Status?.StatusName,
                    TimeSpentMinutes = u.TimeSpentMinutes,
                    UpdatedBy = u.UpdatedBy.Person.FullName,
                    CreatedAt = u.CreatedAt
                }).ToList()
        };
    }
}
