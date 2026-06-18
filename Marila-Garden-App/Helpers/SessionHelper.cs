using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Helpers
{
    public static class SessionHelper
    {
        public static bool IsLoggedIn { get; set; }

        public static string UserName { get; set; } = string.Empty;
    }
}
