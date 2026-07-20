using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services
{
    public class NavigationService : INavigationService
    {
        public async Task GoToAsync(string route)
        {
            if (Shell.Current is null)
                return;

            await Shell.Current.GoToAsync(route);
        }

        public async Task GoToAsync(
            string route,
            IDictionary<string, object> parameters)
        {
            if (Shell.Current is null)
                return;

            await Shell.Current.GoToAsync(route, parameters);
        }

        public async Task GoBackAsync()
        {
            if (Shell.Current is null)
                return;

            await Shell.Current.GoToAsync("..");
        }
    }
}
