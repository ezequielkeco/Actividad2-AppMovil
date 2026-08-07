using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Marila_Garden_App.Data;
using Marila_Garden_App.Models.Assistant;
using Marila_Garden_App.Services;

namespace Marila_Garden_App.ViewModels.Assistant;

public partial class ServiceAssistantViewModel : ObservableObject
{
    private readonly IServiceRecommendationService
        _recommendationService;

    private readonly INavigationService
        _navigationService;

    private readonly List<AssistantQuestion>
        _questions;

    private readonly Dictionary<string, AssistantOption>
        _answers = new();

    [ObservableProperty]
    private int currentQuestionIndex;

    [ObservableProperty]
    private AssistantQuestion? currentQuestion;

    [ObservableProperty]
    private AssistantOption? selectedOption;

    [ObservableProperty]
    private bool isWelcomeVisible = true;

    [ObservableProperty]
    private bool isQuestionVisible;

    [ObservableProperty]
    private bool isProcessing;

    [ObservableProperty]
    private double progress;

    public int QuestionNumber =>
        CurrentQuestionIndex + 1;

    public int TotalQuestions =>
        _questions.Count;

    public string QuestionProgressText =>
        $"Pregunta {QuestionNumber} de {TotalQuestions}";

    public bool CanGoBack =>
        CurrentQuestionIndex > 0;

    public ServiceAssistantViewModel(
        IServiceRecommendationService recommendationService,
        INavigationService navigationService)
    {
        _recommendationService =
            recommendationService;

        _navigationService =
            navigationService;

        _questions =
            ServiceAssistantCatalog
                .GetAll()
                .ToList();

        UpdateProgress();
    }

    [RelayCommand]
    private void Start()
    {
        if (_questions.Count == 0)
            return;

        IsWelcomeVisible = false;
        IsQuestionVisible = true;

        CurrentQuestionIndex = 0;
        CurrentQuestion =
            _questions[CurrentQuestionIndex];

        RestoreCurrentAnswer();

        UpdateProgress();
    }

    [RelayCommand]
    private async Task SelectOption(
        AssistantOption? option)
    {
        if (option is null ||
            CurrentQuestion is null ||
            IsProcessing)
        {
            return;
        }

        SelectedOption = option;

        _answers[CurrentQuestion.Id] =
            option;

        await Task.Delay(250);

        if (CurrentQuestionIndex <
            _questions.Count - 1)
        {
            CurrentQuestionIndex++;

            CurrentQuestion =
                _questions[CurrentQuestionIndex];

            RestoreCurrentAnswer();

            UpdateProgress();

            return;
        }

        await FinishAsync();
    }

    [RelayCommand]
    private void Previous()
    {
        if (CurrentQuestionIndex <= 0)
            return;

        CurrentQuestionIndex--;

        CurrentQuestion =
            _questions[CurrentQuestionIndex];

        RestoreCurrentAnswer();

        UpdateProgress();
    }

    [RelayCommand]
    private void Restart()
    {
        _answers.Clear();

        CurrentQuestionIndex = 0;
        CurrentQuestion = null;
        SelectedOption = null;

        IsProcessing = false;
        IsQuestionVisible = false;
        IsWelcomeVisible = true;

        UpdateProgress();
    }

    private async Task FinishAsync()
    {
        IsQuestionVisible = false;
        IsProcessing = true;

        await Task.Delay(600);

        ServiceRecommendation recommendation =
            _recommendationService
                .GetRecommendation(
                    _answers.Values);

        IsProcessing = false;

        if (recommendation.RecommendedService
            is null)
        {
            Restart();
            return;
        }

        string serviceId =
            Uri.EscapeDataString(
                recommendation
                    .RecommendedService
                    .Id);

        await _navigationService.GoToAsync(
            $"ServiceAssistantResultPage" +
            $"?serviceId={serviceId}");
    }

    private void RestoreCurrentAnswer()
    {
        if (CurrentQuestion is null)
        {
            SelectedOption = null;
            return;
        }

        if (_answers.TryGetValue(
                CurrentQuestion.Id,
                out AssistantOption? option))
        {
            SelectedOption = option;
        }
        else
        {
            SelectedOption = null;
        }
    }

    private void UpdateProgress()
    {
        if (_questions.Count == 0)
        {
            Progress = 0;
            return;
        }

        Progress =
            (CurrentQuestionIndex + 1d)
            / _questions.Count;

        OnPropertyChanged(
            nameof(QuestionNumber));

        OnPropertyChanged(
            nameof(TotalQuestions));

        OnPropertyChanged(
            nameof(QuestionProgressText));

        OnPropertyChanged(
            nameof(CanGoBack));
    }
}