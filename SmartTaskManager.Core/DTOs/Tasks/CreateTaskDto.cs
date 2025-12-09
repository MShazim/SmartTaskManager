using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskManager.Core.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
