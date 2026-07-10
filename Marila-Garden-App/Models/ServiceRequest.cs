using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Marila_Garden_App.Models
{
    public class ServiceRequest
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string FullName { get; set; } = string.Empty;

        [NotNull]
        public string Phone { get; set; } = string.Empty;

        [NotNull]
        public string ServiceType { get; set; } = string.Empty;

        public DateTime DesiredDate { get; set; }

        public string Comments { get; set; } = string.Empty;

        public string Status { get; set; } = "Pendiente";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
