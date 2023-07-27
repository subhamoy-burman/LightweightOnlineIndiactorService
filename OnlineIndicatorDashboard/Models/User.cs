namespace OnlineIndicatorDashboard.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
    }
}
