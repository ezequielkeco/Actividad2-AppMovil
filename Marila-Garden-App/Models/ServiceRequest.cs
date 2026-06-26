using System;
using System.Collections.Generic;
using System.Text;

namespace Marila_Garden_App.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string ServiceType { get; set; } = string.Empty;

        public DateTime DesiredDate { get; set; }

        public string Comments { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendiente";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
