using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync();
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId);
        Task<Todo> CreateTodoAsync(CreateTodoDto createTodoDto);
        Task<Todo?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);
        Task<bool> DeleteTodoAsync(int id);
        Task<IEnumerable<Todo>> GetOverdueTodosAsync();
        Task<IEnumerable<Todo>> GetTodosByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
} 