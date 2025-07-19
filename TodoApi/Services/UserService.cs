using TodoApi.Models;

namespace TodoApi.Services
{
    public class UserService : IUserService
    {
        private readonly InMemoryDataStore _dataStore;

        public UserService()
        {
            _dataStore = InMemoryDataStore.Instance;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await Task.FromResult(_dataStore.Users.OrderBy(u => u.Name));
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await Task.FromResult(_dataStore.Users.FirstOrDefault(u => u.Id == id));
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await Task.FromResult(_dataStore.Users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
        {
            // Check if email already exists
            var existingUser = await GetUserByEmailAsync(createUserDto.Email);
            if (existingUser != null)
            {
                throw new ArgumentException($"User with email {createUserDto.Email} already exists");
            }

            var user = new User
            {
                Id = _dataStore.GetNextUserId(),
                Name = createUserDto.Name,
                Email = createUserDto.Email
            };

            _dataStore.Users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = _dataStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return null;

            // Check if email already exists for another user
            var existingUser = await GetUserByEmailAsync(updateUserDto.Email);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new ArgumentException($"User with email {updateUserDto.Email} already exists");
            }

            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;

            return await Task.FromResult(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = _dataStore.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return false;

            _dataStore.Users.Remove(user);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<User>> SearchUsersByNameAsync(string searchTerm)
        {
            return await Task.FromResult(_dataStore.Users.Where(u => 
                u.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
            ).OrderBy(u => u.Name));
        }

        public async Task<bool> UserExistsAsync(int id)
        {
            return await Task.FromResult(_dataStore.Users.Any(u => u.Id == id));
        }
    }
} 