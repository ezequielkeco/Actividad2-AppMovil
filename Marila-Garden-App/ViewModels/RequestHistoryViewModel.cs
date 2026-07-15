using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels
{
    public partial class RequestsHistoryViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<ServiceRequest> Requests { get; } = new();

        [ObservableProperty]
        private bool isEmpty;

        public RequestsHistoryViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        [RelayCommand]
        public async Task LoadRequests()
        {
            Requests.Clear();

            var requests = await _databaseService.GetRequestsAsync();

            foreach (var request in requests)
            {
                Requests.Add(request);
            }

            IsEmpty = Requests.Count == 0;
        }

        [RelayCommand]
        private async Task EditRequest(ServiceRequest request)
        {
            if (request is null)
                return;

            await Shell.Current.GoToAsync(
                $"//Request?requestId={request.Id}"
            );
        }

        [RelayCommand]
        private async Task DeleteRequest(ServiceRequest request)
        {
            if (request is null)
                return;

            bool confirmed = await Shell.Current.DisplayAlertAsync(
                "Eliminar solicitud",
                $"¿Deseas eliminar la solicitud de {request.ServiceType}?",
                "Eliminar",
                "Cancelar");

            if (!confirmed)
                return;

            await _databaseService.DeleteRequestAsync(request);

            Requests.Remove(request);

            IsEmpty = Requests.Count == 0;
        }
    }
}
