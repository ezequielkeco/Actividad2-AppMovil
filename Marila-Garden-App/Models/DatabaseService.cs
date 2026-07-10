using Marila_Garden_App.Models;
using SQLite;

namespace Marila_Garden_App.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection? _database;

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
        }

        public async Task<List<ServiceRequest>> GetRequestsAsync()
        {
            await InitAsync();

            return await _database!
                .Table<ServiceRequest>()
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