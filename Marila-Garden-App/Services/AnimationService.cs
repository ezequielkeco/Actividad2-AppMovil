using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services;

public class AnimationService : IAnimationService
{
    private const uint PressDuration = 90;
    private const uint ReleaseDuration = 110;
    private const uint FadeDuration = 220;
    private const uint PopDuration = 160;

    public async Task PressAsync(VisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        try
        {
            await element.ScaleToAsync(
                0.96,
                PressDuration,
                Easing.CubicOut);

            await element.ScaleToAsync(
                1.00,
                ReleaseDuration,
                Easing.CubicInOut);
        }
        finally
        {
            element.Scale = 1;
        }
    }

    public async Task FadeInAsync(VisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Opacity = 0;

        try
        {
            await element.FadeToAsync(
                1,
                FadeDuration,
                Easing.CubicOut);
        }
        finally
        {
            element.Opacity = 1;
        }
    }

    public async Task PopAsync(VisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        element.Scale = 0.94;
        element.Opacity = 0;

        try
        {
            await Task.WhenAll(
                element.ScaleToAsync(
                    1,
                    PopDuration,
                    Easing.CubicOut),

                element.FadeToAsync(
                    1,
                    PopDuration,
                    Easing.CubicOut));
        }
        finally
        {
            element.Scale = 1;
            element.Opacity = 1;
        }
    }
}
