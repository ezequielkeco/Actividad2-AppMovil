using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Marila_Garden_App.Messages;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels
{
    public partial class RequestsHistoryViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;
        private readonly ISessionService _sessionService;

        public string UserName =>
            _sessionService.CurrentUser?.FullName ?? "Usuario";

        public ObservableCollection<ServiceRequest> Requests { get; } = new();

        [ObservableProperty]
        private bool isEmpty;

        public RequestsHistoryViewModel(
               DatabaseService databaseService,
               IDialogService dialogService,
               INavigationService navigationService,
               ISessionService sessionService)
        {
            _databaseService = databaseService;
            _dialogService = dialogService;
            _navigationService = navigationService;
            _sessionService = sessionService;

            WeakReferenceMessenger.Default.Register<
                ServiceRequestCreatedMessage>(
                this,
                (_, message) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Requests.Insert(0, message.Request);
                        IsEmpty = false;
                    });
                });

            WeakReferenceMessenger.Default.Register<
                ServiceRequestUpdatedMessage>(
                this,
                (_, message) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ServiceRequest? existing =
                            Requests.FirstOrDefault(
                                item => item.Id == message.Request.Id);

                        if (existing is null)
                        {
                            Requests.Insert(0, message.Request);
                            IsEmpty = false;
                            return;
                        }

                        int index = Requests.IndexOf(existing);

                        Requests[index] = message.Request;
                    });
                });

            WeakReferenceMessenger.Default.Register<
                ServiceRequestDeletedMessage>(
                this,
                (_, message) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ServiceRequest? existing =
                            Requests.FirstOrDefault(
                                item => item.Id == message.RequestId);

                        if (existing is not null)
                            Requests.Remove(existing);

                        IsEmpty = Requests.Count == 0;
                    });
                });
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

            await _navigationService.GoToAsync(
                $"//Request?requestId={request.Id}");
        }

        [RelayCommand]
        public async Task<bool> ConfirmDeleteRequestAsync(
            ServiceRequest request)
        {
            if (request is null)
                return false;

            return await _dialogService.ConfirmAsync(
                "Eliminar solicitud",
                $"¿Deseas eliminar la solicitud de {request.ServiceType}?",
                "Eliminar",
                "Cancelar");
        }

        public async Task DeleteConfirmedRequestAsync(
            ServiceRequest request)
        {
            if (request is null)
                return;

            await _databaseService.DeleteRequestAsync(request);

            Requests.Remove(request);

            IsEmpty = Requests.Count == 0;
        }

        public void RefreshSessionData()
        {
            OnPropertyChanged(nameof(UserName));
        }
    }
}