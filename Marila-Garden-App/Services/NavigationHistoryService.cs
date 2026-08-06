using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services;

public class NavigationHistoryService
    : INavigationHistoryService
{
    private readonly Stack<string> _routes = new();

    public int Count =>
        _routes.Count;

    public void Push(string route)
    {
        if (string.IsNullOrWhiteSpace(route))
            return;

        string normalizedRoute =
            NormalizeRoute(route);

        if (string.IsNullOrWhiteSpace(normalizedRoute))
            return;

        if (_routes.TryPeek(out string? currentRoute) &&
            string.Equals(
                currentRoute,
                normalizedRoute,
                StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        _routes.Push(normalizedRoute);
    }

    public string? Pop()
    {
        if (_routes.Count == 0)
            return null;

        return _routes.Pop();
    }

    public string? Peek()
    {
        if (_routes.Count == 0)
            return null;

        return _routes.Peek();
    }

    public string? PeekPrevious()
    {
        if (_routes.Count < 2)
            return null;

        return _routes
            .Skip(1)
            .FirstOrDefault();
    }

    public void Clear()
    {
        _routes.Clear();
    }

    private static string NormalizeRoute(string route)
    {
        return route
            .Split('?')[0]
            .Trim()
            .TrimEnd('/');
    }
}
