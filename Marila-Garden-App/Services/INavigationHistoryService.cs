using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services;

public interface INavigationHistoryService
{
    int Count { get; }

    void Push(string route);

    string? Pop();

    string? Peek();

    string? PeekPrevious();

    void Clear();
}
