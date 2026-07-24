using System;
using System.Collections.Generic;
using System.Text;
using Marila_Garden_App.Models;

namespace Marila_Garden_App.Services
{
    public interface ISessionService
    {
        bool IsLoggedIn { get; }

        User? CurrentUser { get; }

        void StartSession(User user);

        void EndSession();
    }
}
