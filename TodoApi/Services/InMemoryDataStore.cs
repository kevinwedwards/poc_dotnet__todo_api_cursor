using TodoApi.Models;
using TodoApi.Configuration;

namespace TodoApi.Services
{
    public class InMemoryDataStore
    {
        private static InMemoryDataStore? _instance;
        private static readonly object _lock = new();

        public static InMemoryDataStore Instance => _instance ?? throw new InvalidOperationException("InMemoryDataStore not initialized. Call Initialize() first.");

        private InMemoryDataStore()
        {
            // Private constructor - use Initialize() to create instance
        }

        public static void Initialize(DataStoreOptions options)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new InMemoryDataStore();
                    if (options.InitializeSampleData)
                    {
                        _instance.InitializeData();
                    }
                }
            }
        }

        public List<User> Users { get; private set; } = new();
        public List<Todo> Todos { get; private set; } = new();
        
        private int _nextUserId = 1;
        private int _nextTodoId = 1;

        public void InitializeData()
        {
            // Initialize sample users
            Users.Add(new User
            {
                Id = _nextUserId++,
                Name = "John Doe",
                Email = "john.doe@example.com"
            });

            Users.Add(new User
            {
                Id = _nextUserId++,
                Name = "Jane Smith",
                Email = "jane.smith@example.com"
            });

            Users.Add(new User
            {
                Id = _nextUserId++,
                Name = "Bob Johnson",
                Email = "bob.johnson@example.com"
            });

            // Initialize sample todos
            Todos.Add(new Todo
            {
                Id = _nextTodoId++,
                Order = 1,
                CreatedByUserId = 1, // John Doe
                CreatedOn = DateTime.UtcNow.AddDays(-1),
                Description = "Complete project documentation",
                PlannedDate = DateTime.UtcNow.AddDays(7),
                DueDate = DateTime.UtcNow.AddDays(14)
            });

            Todos.Add(new Todo
            {
                Id = _nextTodoId++,
                Order = 2,
                CreatedByUserId = 2, // Jane Smith
                CreatedOn = DateTime.UtcNow.AddHours(-2),
                Description = "Review code changes",
                PlannedDate = DateTime.UtcNow.AddDays(2),
                DueDate = DateTime.UtcNow.AddDays(5)
            });

            Todos.Add(new Todo
            {
                Id = _nextTodoId++,
                Order = 3,
                CreatedByUserId = 1, // John Doe
                CreatedOn = DateTime.UtcNow.AddHours(-1),
                Description = "Set up development environment",
                PlannedDate = DateTime.UtcNow.AddDays(1),
                DueDate = DateTime.UtcNow.AddDays(3)
            });
        }

        public int GetNextUserId()
        {
            lock (_lock)
            {
                return _nextUserId++;
            }
        }

        public int GetNextTodoId()
        {
            lock (_lock)
            {
                return _nextTodoId++;
            }
        }

        public void ClearAllData()
        {
            lock (_lock)
            {
                Users.Clear();
                Todos.Clear();
                _nextUserId = 1;
                _nextTodoId = 1;
                InitializeData();
            }
        }
    }
} 