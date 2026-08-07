using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Models.Assistant;

public class AssistantQuestion
{
    public string Id { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public IReadOnlyList<AssistantOption> Options { get; set; } =
        Array.Empty<AssistantOption>();
}
