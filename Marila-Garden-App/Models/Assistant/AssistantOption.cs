using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Models.Assistant;

public class AssistantOption
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public Dictionary<string, int> Scores { get; set; } =
        new();
}
