using System;
using System.Collections.Generic;
using System.Text;
using Marila_Garden_App.Models;

namespace Marila_Garden_App.Services
{
    public class SessionService : ISessionService
    {
        public User? CurrentUser { get; private set; }

        public bool IsLoggedIn =>
            CurrentUser is not null;

        public void StartSession(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            CurrentUser = user;
        }

        public void EndSession()
        {
            CurrentUser = null;
        }
    }
}
