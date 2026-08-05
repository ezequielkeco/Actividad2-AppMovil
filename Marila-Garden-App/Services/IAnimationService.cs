using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services;

public interface IAnimationService
{
    Task PressAsync(VisualElement element);

    Task FadeInAsync(VisualElement element);

    Task PopAsync(VisualElement element);
}
