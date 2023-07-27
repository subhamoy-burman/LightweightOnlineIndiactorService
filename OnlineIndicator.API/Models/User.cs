using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineIndicator.API.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime LastHeartbeatTime { get; set; }
    }
}
