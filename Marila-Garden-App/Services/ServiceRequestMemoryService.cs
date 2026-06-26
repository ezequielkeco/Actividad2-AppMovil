using Marila_Garden_App.Models;

namespace Marila_Garden_App.Services
{
    public static class ServiceRequestMemoryService
    {
        private static readonly List<ServiceRequest> _requests = new();
        private static int _nextId = 1;

        public static List<ServiceRequest> GetAll()
        {
            return _requests
                .OrderByDescending(request => request.CreatedAt)
                .ToList();
        }

        public static ServiceRequest? GetById(int id)
        {
            return _requests.FirstOrDefault(request => request.Id == id);
        }

        public static void Add(ServiceRequest request)
        {
            request.Id = _nextId++;
            request.CreatedAt = DateTime.Now;
            request.Status = "Pendiente";

            _requests.Add(request);
        }

        public static void Update(ServiceRequest updatedRequest)
        {
            var existingRequest = GetById(updatedRequest.Id);

            if (existingRequest is null)
                return;

            existingRequest.FullName = updatedRequest.FullName;
            existingRequest.Phone = updatedRequest.Phone;
            existingRequest.ServiceType = updatedRequest.ServiceType;
            existingRequest.DesiredDate = updatedRequest.DesiredDate;
            existingRequest.Comments = updatedRequest.Comments;
            existingRequest.Status = updatedRequest.Status;
        }

        public static void Delete(int id)
        {
            var request = GetById(id);

            if (request is not null)
            {
                _requests.Remove(request);
            }
        }

        public static void Clear()
        {
            _requests.Clear();
            _nextId = 1;
        }
    }
}