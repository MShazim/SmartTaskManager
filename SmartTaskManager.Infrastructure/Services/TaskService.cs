using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTaskManager.Core.Domain;
using SmartTaskManager.Core.DTOs.Tasks;
using SmartTaskManager.Core.Interfaces;
using SmartTaskManager.Infrastructure.Data;

namespace SmartTaskManager.Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }
    
        public async Task<Guid> CreateTaskAsync(Guid userId, CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                TaskId = Guid.NewGuid(),
                UserId = userId,
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Status = "ToDo",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task.TaskId;
        }

        public async Task<IEnumerable<TaskResponseDto>> GetUserTaskAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TaskResponseDto
                {
                    TaskId = t.TaskId,
                    Title = t.Title,
                    Description= t.Description,
                    Status = t.Status,
                    DueDate  = t.DueDate,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.CreatedAt,
                })
                .ToListAsync();
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == taskId && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return null;
            }

            return new TaskResponseDto
            {
                TaskId = task.TaskId,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.CreatedAt,

            };
        }

        public async Task<bool> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == taskId && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return false;
            }

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.DueDate = dto.DueDate;
            task.UpdatedAt = DateTime.UtcNow;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .Where(t => t.TaskId == taskId && t.UserId == userId)
                .FirstOrDefaultAsync();

            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
