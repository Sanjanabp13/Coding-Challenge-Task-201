using CodingChallenge.Models;

namespace CodingChallenge.Interface
{
    public interface ITasksRepository
    {
        Task AddAsync(Tasks tasks);
        Task UpdateAsync(Tasks tasks);
        Task DeleteAsync(int taskId);
        Task<Tasks> GetByIdAsync(int taskId);
        Task<IEnumerable<Tasks>> GetAllAsync();
    }
}
