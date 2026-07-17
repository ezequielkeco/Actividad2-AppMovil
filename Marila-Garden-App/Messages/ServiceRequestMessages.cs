using System;
using System.Collections.Generic;
using System.Text;
using Marila_Garden_App.Models;

namespace Marila_Garden_App.Messages
{
    public sealed record ServiceRequestCreatedMessage(
        ServiceRequest Request);

    public sealed record ServiceRequestUpdatedMessage(
        ServiceRequest Request);

    public sealed record ServiceRequestDeletedMessage(
        int RequestId);
}