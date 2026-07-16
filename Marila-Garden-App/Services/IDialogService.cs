using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services
{
    public interface IDialogService
    {
        Task<bool> ConfirmAsync(
            string title,
            string message,
            string accept,
            string cancel);

        Task ShowAlertAsync(
            string title,
            string message,
            string cancel);
    }
}
