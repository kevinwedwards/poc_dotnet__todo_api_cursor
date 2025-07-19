using TodoApi.Models;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly InMemoryDataStore _dataStore;
        private readonly IUserService _userService;

        public TodoService(IUserService userService)
        {
            _dataStore = InMemoryDataStore.Instance;
            _userService = userService;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync()
        {
            return await Task.FromResult(_dataStore.Todos.OrderBy(t => t.Order));
        }

        public async Task<Todo?> GetTodoByIdAsync(int id)
        {
            return await Task.FromResult(_dataStore.Todos.FirstOrDefault(t => t.Id == id));
        }

        public async Task<IEnumerable<Todo>> GetTodosByUserIdAsync(int userId)
        {
            return await Task.FromResult(_dataStore.Todos.Where(t => t.CreatedByUserId == userId).OrderBy(t => t.Order));
        }

        public async Task<Todo> CreateTodoAsync(CreateTodoDto createTodoDto)
        {
            // Validate that the user exists
            var user = await _userService.GetUserByIdAsync(createTodoDto.CreatedByUserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {createTodoDto.CreatedByUserId} does not exist");
            }

            var todo = new Todo
            {
                Id = _dataStore.GetNextTodoId(),
                Order = createTodoDto.Order,
                CreatedByUserId = createTodoDto.CreatedByUserId,
                CreatedOn = DateTime.UtcNow,
                Description = createTodoDto.Description,
                PlannedDate = createTodoDto.PlannedDate,
                DueDate = createTodoDto.DueDate
            };

            _dataStore.Todos.Add(todo);
            return await Task.FromResult(todo);
        }

        public async Task<Todo?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
        {
            var todo = _dataStore.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
                return null;

            todo.Order = updateTodoDto.Order;
            todo.Description = updateTodoDto.Description;
            todo.PlannedDate = updateTodoDto.PlannedDate;
            todo.DueDate = updateTodoDto.DueDate;

            return await Task.FromResult(todo);
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = _dataStore.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
                return false;

            _dataStore.Todos.Remove(todo);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Todo>> GetOverdueTodosAsync()
        {
            var now = DateTime.UtcNow;
            return await Task.FromResult(_dataStore.Todos.Where(t => t.DueDate.HasValue && t.DueDate.Value < now).OrderBy(t => t.DueDate));
        }

        public async Task<IEnumerable<Todo>> GetTodosByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await Task.FromResult(_dataStore.Todos.Where(t => 
                (t.PlannedDate.HasValue && t.PlannedDate.Value >= startDate && t.PlannedDate.Value <= endDate) ||
                (t.DueDate.HasValue && t.DueDate.Value >= startDate && t.DueDate.Value <= endDate)
            ).OrderBy(t => t.Order));
        }
    }
} 