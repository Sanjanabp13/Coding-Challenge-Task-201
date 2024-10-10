using CodingChallenge.Exceptions;
using CodingChallenge.Interface;
using CodingChallenge.Models;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Repository
{
    public class TasksRepository:ITasksRepository
    {
        private readonly TaskDBcontext _context;

        public TasksRepository(TaskDBcontext context)
        {
            _context = context;
        }
        public async Task AddAsync(Tasks tasks)
        {
            try
            {
                await _context.Tasks.AddAsync(tasks);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Error adding task to the database.", ex);
            }
        }

        public async Task UpdateAsync(Tasks tasks)
        {
            try
            {
                var existing = await _context.Tasks.FindAsync(tasks.TaskID);
                if (existing == null)
                {
                    throw new NotFoundException($"task with ID {tasks.TaskID} not found.");
                }

                existing.Title = tasks.Title;
                existing.Description = tasks.Description;
                existing.DueDate = tasks.DueDate;
                existing.Priority = tasks.Priority;
                existing.Status = tasks.Status;

                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new RepositoryException("Concurrency error occurred while updating task.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error updating task.", ex);
            }
        }

        public async Task DeleteAsync(int taskId)
        {
            try
            {
                var tasks = await _context.Tasks.FindAsync(taskId);
                if (tasks == null)
                {
                    throw new NotFoundException($"tasks with ID {taskId} not found.");
                }

                _context.Tasks.Remove(tasks);
                await _context.SaveChangesAsync();
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new RepositoryException("Error deleting tasks from the database.", ex);
            }
        }

        public async Task<Tasks> GetByIdAsync(int taskId)
        {
            try
            {
                var tasks = await _context.Tasks.FindAsync(taskId);
                if (tasks == null)
                {
                    throw new NotFoundException($"tasks with ID {taskId} not found.");
                }

                return tasks;
            }
            catch (NotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving tasks by ID.", ex);
            }
        }

        public async Task<IEnumerable<Tasks>> GetAllAsync()
        {
            try
            {
                return await _context.Tasks.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new RepositoryException("Error retrieving all tasks.", ex);
            }
        }

        


    }
}
