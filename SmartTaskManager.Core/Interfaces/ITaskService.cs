using SmartTaskManager.Core.DTOs.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartTaskManager.Core.Interfaces
{
    public interface ITaskService
    {
        Task<Guid> CreateTaskAsync(Guid userId, CreateTaskDto dto);
        Task<IEnumerable<TaskResponseDto>> GetUserTaskAsync(Guid userId);
        Task<TaskResponseDto> GetTaskByIdAsync(Guid userId, Guid taskId);
        Task<bool> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);
    }
}
