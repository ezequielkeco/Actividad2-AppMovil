using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Marila_Garden_App.Data;
using Marila_Garden_App.Messages;
using Marila_Garden_App.Models;
using Marila_Garden_App.Services;
using Marila_Garden_App.ViewModels.Base;

namespace Marila_Garden_App.ViewModels.Request;

public partial class RequestViewModel : AuthenticatedFormViewModelBase
{
    private readonly DatabaseService _databaseService;
    private readonly INavigationService _navigationService;

    private int _editingRequestId;

    public RequestViewModel(
           DatabaseService databaseService,
           IDialogService dialogService,
           INavigationService navigationService,
           ISessionService sessionService)
           : base(dialogService, sessionService)
    {
        _databaseService = databaseService;
        _navigationService = navigationService;

        ConfigureCreateMode(
            "Nueva solicitud",
            "Enviar solicitud");
    }

    public void PrepareForService(string serviceId)
    {
        ResetToCreateMode();

        ServiceInfo? service =
            ServiceCatalog.GetById(serviceId);

        if (service is null)
            return;

        SelectedServiceType = service.Name;

        HasUnsavedChanges = false;
    }

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string selectedServiceType = string.Empty;

    [ObservableProperty]
    private DateTime selectedDate = DateTime.Today;

    [ObservableProperty]
    private string comments = string.Empty;

    [ObservableProperty]
    private string fullNameError = string.Empty;

    [ObservableProperty]
    private string phoneError = string.Empty;

    [ObservableProperty]
    private string serviceTypeError = string.Empty;

    [ObservableProperty]
    private string dateError = string.Empty;

    [ObservableProperty]
    private string commentsError = string.Empty;

    public string SelectedDateDisplay =>
        SelectedDate.ToString("dd/MM/yyyy");

    public List<string> ServiceTypes { get; } = new()
        {
            "Diseño de jardín",
            "Mantenimiento",
            "Poda profesional",
            "Plantación"
        };

    partial void OnFullNameChanged(string value)
    {
        FullNameError = string.Empty;
        SuccessMessage = string.Empty;
        HasUnsavedChanges = true;
    }

    partial void OnPhoneChanged(string value)
    {
        PhoneError = string.Empty;
        SuccessMessage = string.Empty;
        HasUnsavedChanges = true;
    }

    partial void OnSelectedServiceTypeChanged(string value)
    {
        ServiceTypeError = string.Empty;
        SuccessMessage = string.Empty;
        HasUnsavedChanges = true;
    }

    partial void OnSelectedDateChanged(DateTime value)
    {
        DateError = string.Empty;
        SuccessMessage = string.Empty;
        HasUnsavedChanges = true;

        OnPropertyChanged(nameof(SelectedDateDisplay));
    }

    partial void OnCommentsChanged(string value)
    {
        CommentsError = string.Empty;
        SuccessMessage = string.Empty;
        HasUnsavedChanges = true;
    }

    public async Task LoadRequestForEditAsync(int requestId)
    {
        ClearMessages();

        ServiceRequest? request =
            await _databaseService.GetRequestByIdAsync(requestId);

        User? currentUser = SessionService.CurrentUser;

        if (request is null || currentUser is null)
        {
            ResetToCreateMode();
            return;
        }

        if (request.UserId != currentUser.Id)
        {
            await ShowAlertAsync(
                "Acceso no autorizado",
                "No tienes permisos para acceder a esta solicitud",
                "Aceptar");

            ResetToCreateMode();

            await _navigationService.GoToAsync("//RequestsHistory");

            return;
        }

        _editingRequestId = request.Id;

        ConfigureEditMode(
            "Editar solicitud",
            "Guardar cambios");

        FullName = request.FullName;
        Phone = request.Phone;
        SelectedServiceType = request.ServiceType;
        SelectedDate = request.DesiredDate;
        Comments = request.Comments;

        HasUnsavedChanges = false;
    }

    public void ResetToCreateMode()
    {
        _editingRequestId = 0;

        ConfigureCreateMode(
            "Nueva solicitud",
            "Enviar solicitud");

        ClearMessages();
        ClearForm();

        HasUnsavedChanges = false;
    }

    [RelayCommand]
    private async Task SaveRequest()
    {
        ClearMessages();

        if (!ValidateForm())
            return;

        if (IsEditMode)
        {
            await UpdateExistingRequestAsync();
            return;
        }

        await CreateNewRequestAsync();
    }

    private async Task CreateNewRequestAsync()
    {
        var request = new ServiceRequest
        {
            UserId = SessionService.CurrentUser!.Id,

            FullName = FullName.Trim(),
            Phone = Phone.Trim(),
            ServiceType = SelectedServiceType,
            DesiredDate = SelectedDate,
            Comments = Comments.Trim()
        };

        await _databaseService.AddRequestAsync(request);

        WeakReferenceMessenger.Default.Send(
            new ServiceRequestCreatedMessage(request));

        ClearForm();

        HasUnsavedChanges = false;
        SuccessMessage = "Solicitud registrada correctamente";

        _ = HideSuccessMessageAsync();
    }

    private async Task UpdateExistingRequestAsync()
    {
        ServiceRequest? existingRequest =
            await _databaseService.GetRequestByIdAsync(_editingRequestId);

        User? currentUser = SessionService.CurrentUser;

        if (existingRequest is null || currentUser is null)
            return;

        if (existingRequest.UserId != currentUser.Id)
        {
            await ShowAlertAsync(
                "Acceso no autorizado",
                "No tienes permisos para modificar esta solicitud",
                "Aceptar");

            ResetToCreateMode();

            await _navigationService.GoToAsync("//RequestsHistory");

            return;
        }

        existingRequest.FullName = FullName.Trim();
        existingRequest.Phone = Phone.Trim();
        existingRequest.ServiceType = SelectedServiceType;
        existingRequest.DesiredDate = SelectedDate;
        existingRequest.Comments = Comments.Trim();

        await _databaseService.UpdateRequestAsync(existingRequest);

        WeakReferenceMessenger.Default.Send(
            new ServiceRequestUpdatedMessage(existingRequest));

        ResetToCreateMode();

        HasUnsavedChanges = false;
        SuccessMessage = "Solicitud actualizada correctamente";

        await Task.Delay(3000);

        await _navigationService.GoToAsync("//RequestsHistory");
    }

    [RelayCommand]
    private async Task DeleteRequest()
    {
        if (!IsEditMode || _editingRequestId <= 0)
            return;

        bool confirmed = await ConfirmAsync(
            "Eliminar solicitud",
            "¿Estás seguro de que deseas eliminar esta solicitud? Esta acción no se puede deshacer.",
            "Eliminar",
            "Cancelar");

        if (!confirmed)
            return;

        ServiceRequest? request =
            await _databaseService.GetRequestByIdAsync(_editingRequestId);

        User? currentUser = SessionService.CurrentUser;

        if (request is null || currentUser is null)
            return;

        if (request.UserId != currentUser.Id)
        {
            await ShowAlertAsync(
                "Acceso no autorizado",
                "No tienes permisos para eliminar esta solicitud",
                "Aceptar");

            ResetToCreateMode();

            await _navigationService.GoToAsync("//RequestsHistory");

            return;
        }

        await _databaseService.DeleteRequestAsync(request);

        WeakReferenceMessenger.Default.Send(
            new ServiceRequestDeletedMessage(request.Id));

        HasUnsavedChanges = false;

        ResetToCreateMode();

        HasUnsavedChanges = false;

        await _navigationService.GoToAsync("//RequestsHistory");
    }

    private bool ValidateForm()
    {
        bool isValid = true;

        if (string.IsNullOrWhiteSpace(FullName))
        {
            FullNameError = "El nombre completo es obligatorio";
            isValid = false;
        }
        else if (FullName.Trim().Length < 5)
        {
            FullNameError = "El nombre debe tener al menos 5 caracteres";
            isValid = false;
        }
        else if (FullName.Any(char.IsDigit))
        {
            FullNameError = "El nombre no debe contener números";
            isValid = false;
        }

        if (string.IsNullOrWhiteSpace(Phone))
        {
            PhoneError = "El teléfono es obligatorio";
            isValid = false;
        }
        else
        {
            string cleanPhone = new string(
                Phone.Where(char.IsDigit).ToArray());

            if (cleanPhone.Length != 10)
            {
                PhoneError = "El teléfono debe tener 10 dígitos";
                isValid = false;
            }
        }

        if (string.IsNullOrWhiteSpace(SelectedServiceType))
        {
            ServiceTypeError =
                "Debes seleccionar un tipo de servicio";

            isValid = false;
        }

        if (SelectedDate.Date < DateTime.Today)
        {
            DateError =
                "La fecha no puede ser anterior al día de hoy";

            isValid = false;
        }

        if (!string.IsNullOrWhiteSpace(Comments) &&
            Comments.Length > 300)
        {
            CommentsError =
                "Los comentarios no deben superar los 300 caracteres";

            isValid = false;
        }

        return isValid;
    }

    private void ClearMessages()
    {
        FullNameError = string.Empty;
        PhoneError = string.Empty;
        ServiceTypeError = string.Empty;
        DateError = string.Empty;
        CommentsError = string.Empty;
        SuccessMessage = string.Empty;
    }

    private void ClearForm()
    {
        FullName = string.Empty;
        Phone = string.Empty;
        SelectedServiceType = string.Empty;
        SelectedDate = DateTime.Today;
        Comments = string.Empty;
    }
}
