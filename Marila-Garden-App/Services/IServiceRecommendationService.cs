using Marila_Garden_App.Models.Assistant;

namespace Marila_Garden_App.Services;

public interface IServiceRecommendationService
{
    ServiceRecommendation GetRecommendation(
        IEnumerable<AssistantOption> selectedOptions);
}