using Marila_Garden_App.Models;
using SQLite;

namespace Marila_Garden_App.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

        public async Task<int> CreateUserAsync(User user)
        {
            await InitAsync();

            user.UserName = user.UserName.Trim().ToLowerInvariant();
            user.Email = user.Email.Trim().ToLowerInvariant();
            user.FullName = user.FullName.Trim();

            return await _database!.InsertAsync(user);
        }

        public async Task<User?> GetUserByUserNameAsync(string userName)
        {
            await InitAsync();

            string normalizedUserName =
                userName.Trim().ToLowerInvariant();

            return await _database!
                .Table<User>()
                .FirstOrDefaultAsync(user =>
                    user.UserName == normalizedUserName);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            await InitAsync();

            string normalizedEmail =
                email.Trim().ToLowerInvariant();

            return await _database!
                .Table<User>()
                .FirstOrDefaultAsync(user =>
                    user.Email == normalizedEmail);
        }

        public async Task<User?> GetUserByUserNameOrEmailAsync(string value)
        {
            await InitAsync();

            string normalizedValue =
                value.Trim().ToLowerInvariant();

            User? user = await _database!
                .Table<User>()
                .FirstOrDefaultAsync(user =>
                    user.UserName == normalizedValue);

            if (user is not null)
                return user;

            return await _database!
                .Table<User>()
                .FirstOrDefaultAsync(user =>
                    user.Email == normalizedValue);
        }

        public async Task<bool> UserNameExistsAsync(string userName)
        {
            User? user = await GetUserByUserNameAsync(userName);

            return user is not null;
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            User? user = await GetUserByEmailAsync(email);

            return user is not null;
        }

        private async Task InitAsync()
        {
            if (_database is not null)
                return;

            string databasePath = Path.Combine(
                FileSystem.AppDataDirectory,
                "marila_garden.db3"
            );

            _database = new SQLiteAsyncConnection(databasePath);

            await _database.CreateTableAsync<ServiceRequest>();
            await _database.CreateTableAsync<User>();
        }

        public async Task<List<ServiceRequest>> GetRequestsAsync()
        {
            await InitAsync();

            return await _database!
                .Table<ServiceRequest>()
                .OrderByDescending(request => request.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<ServiceRequest>> GetRequestsByUserAsync(int userId)
        {
            await InitAsync();

            return await _database!
                .Table<ServiceRequest>()
                .Where(request => request.UserId == userId)
                .OrderByDescending(request => request.CreatedAt)
                .ToListAsync();
        }

        public async Task<ServiceRequest?> GetRequestByIdAsync(int id)
        {
            await InitAsync();

            return await _database!
                .Table<ServiceRequest>()
                .Where(request => request.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> AddRequestAsync(ServiceRequest request)
        {
            await InitAsync();

            request.CreatedAt = DateTime.Now;
            request.Status = "Pendiente";

            return await _database!.InsertAsync(request);
        }

        public async Task<int> UpdateRequestAsync(ServiceRequest request)
        {
            await InitAsync();

            return await _database!.UpdateAsync(request);
        }

        public async Task<int> DeleteRequestAsync(ServiceRequest request)
        {
            await InitAsync();

            return await _database!.DeleteAsync(request);
        }
    }
}