using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Services
{
    public class DialogService : IDialogService
    {
        public async Task<bool> ConfirmAsync(
            string title,
            string message,
            string accept,
            string cancel)
        {
            if (Shell.Current is null)
                return false;

            return await Shell.Current.DisplayAlertAsync(
                title,
                message,
                accept,
                cancel);
        }

        public async Task ShowAlertAsync(
            string title,
            string message,
            string cancel)
        {
            if (Shell.Current is null)
                return;

            await Shell.Current.DisplayAlertAsync(
                title,
                message,
                cancel);
        }
    }
}
