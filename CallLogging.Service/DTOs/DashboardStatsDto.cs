namespace CallLogging.Services.DTOs
{
    public class DashboardStatsDto
    {
        public int TotalOpen { get; set; }
        public int Unassigned { get; set; }
        public Dictionary<string, int> OpenByPriority { get; set; } = new();
    }
}
