using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services
{
    public interface INavigationService
    {
        Task GoToAsync(string route);

        Task GoToAsync(
            string route,
            IDictionary<string, object> parameters);

        Task GoBackAsync();
    }
}
