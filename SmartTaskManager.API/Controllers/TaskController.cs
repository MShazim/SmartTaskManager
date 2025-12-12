using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTaskManager.Core.DTOs.Tasks;
using SmartTaskManager.Core.Interfaces;
using System.Security.Claims;

namespace SmartTaskManager.API.Controllers
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        //Helper method to extract UserId from JWT token
        private Guid GetUserId()
        {
            var userId = User.FindFirst("UserId")?.Value;
            return Guid.Parse(userId);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            var userId = GetUserId();
            var taskId = await _taskService.CreateTaskAsync(userId, dto);
            return Ok(new
            {
                message = "Task created successfully",
                taskId
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetUserTaskAsync(userId);
            return Ok(tasks);
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(Guid taskId)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(userId, taskId);

            if (task == null)
            {
                return NotFound(new
                {
                    message = "Task not found"
                });
            }

            return Ok(task);
        }

        [HttpPut("{taskId}")]
        public async Task<IActionResult> UpdateTask(Guid taskId, UpdateTaskDto dto)
        {
            var userId = GetUserId();
            var success = await _taskService.UpdateTaskAsync(userId,taskId,dto);

            if (!success)
            {
                return NotFound(new
                {
                    message = "Task not found or not yours"
                });
            }

            return Ok(new
            {
                message = "Task updated successfully"
            });
        }

        [HttpDelete("{taskId}")]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            var userId = GetUserId();
            var success = await _taskService.DeleteTaskAsync(userId,taskId);

            if(!success)
            {
                return NotFound(new
                {
                    message = "Task not found or not yours"
                });
            }

            return Ok(new
            {
                message = "Task deleted successfully"
            });
        }

        [HttpGet("debug-count")]
        public async Task<IActionResult> DebugCount()
        {
            var userId = GetUserId();
            var tasks = await _taskService.GetUserTaskAsync(userId);
            return Ok(new { count = tasks.Count()});
        }
    }
}
