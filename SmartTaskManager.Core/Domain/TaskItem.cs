using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskManager.Core.Domain
{
    public class TaskItem
    {
        public Guid TaskId { get; set; }
        
        // Foreign key reference to User
        public Guid UserId{ get; set; }
        public User User{ get; set; }
        public string Title{ get; set; }
        public string Description{ get; set; }
        public string Status{ get; set; } = "ToDo"; // ToDo, InProgress, Completed, Overdue
        public DateTime? DueDate{ get; set; }
        public DateTime CreatedAt{ get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt{ get; set; } = DateTime.UtcNow;

    }
}
