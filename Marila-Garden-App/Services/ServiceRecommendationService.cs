using Marila_Garden_App.Data;
using Marila_Garden_App.Models;
using Marila_Garden_App.Models.Assistant;

namespace Marila_Garden_App.Services;

public class ServiceRecommendationService
    : IServiceRecommendationService
{
    public ServiceRecommendation GetRecommendation(
        IEnumerable<AssistantOption> selectedOptions)
    {
        Dictionary<string, int> totalScores =
            new(StringComparer.OrdinalIgnoreCase);

        foreach (AssistantOption option in selectedOptions)
        {
            foreach (var score in option.Scores)
            {
                if (!totalScores.ContainsKey(score.Key))
                {
                    totalScores[score.Key] = 0;
                }

                totalScores[score.Key] += score.Value;
            }
        }

        var rankedServices = totalScores
            .OrderByDescending(item => item.Value)
            .ToList();

        ServiceInfo? recommendedService = null;
        ServiceInfo? alternativeService = null;

        if (rankedServices.Count > 0)
        {
            recommendedService =
                ServiceCatalog.GetById(
                    rankedServices[0].Key);
        }

        if (rankedServices.Count > 1)
        {
            alternativeService =
                ServiceCatalog.GetById(
                    rankedServices[1].Key);
        }

        return new ServiceRecommendation
        {
            RecommendedService = recommendedService,
            AlternativeService = alternativeService,
            Reasons = BuildReasons(
                selectedOptions,
                recommendedService)
        };
    }

    private static IReadOnlyList<string> BuildReasons(
        IEnumerable<AssistantOption> selectedOptions,
        ServiceInfo? recommendedService)
    {
        if (recommendedService is null)
            return Array.Empty<string>();

        List<string> reasons = new();

        foreach (AssistantOption option in selectedOptions)
        {
            if (!option.Scores.TryGetValue(
                    recommendedService.Id,
                    out int score))
            {
                continue;
            }

            if (score <= 0)
                continue;

            reasons.Add(option.Title);
        }

        return reasons;
    }
}