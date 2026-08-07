using System;
using System.Collections.Generic;
using System.Text;

using Marila_Garden_App.Models;

namespace Marila_Garden_App.Models.Assistant;

public class ServiceRecommendation
{
    public ServiceInfo? RecommendedService { get; set; }

    public ServiceInfo? AlternativeService { get; set; }

    public IReadOnlyList<string> Reasons { get; set; } =
        Array.Empty<string>();
}
