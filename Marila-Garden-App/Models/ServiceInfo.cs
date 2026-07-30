using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Models;

public class ServiceInfo
{
    public string Id { get; init; } = string.Empty;

    public string Name { get; init; } = string.Empty;

    public string ShortDescription { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string EstimatedDuration { get; init; } = string.Empty;

    public string CoverImage { get; init; } = string.Empty;

    public string Icon { get; init; } = string.Empty;

    public IReadOnlyList<string> Includes { get; init; } =
        Array.Empty<string>();

    public IReadOnlyList<string> Benefits { get; init; } =
        Array.Empty<string>();

    public IReadOnlyList<string> Images { get; init; } =
        Array.Empty<string>();
}
