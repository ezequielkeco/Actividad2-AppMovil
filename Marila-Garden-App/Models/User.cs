using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Marila_Garden_App.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull]
        public string FullName { get; set; } = string.Empty;

        [NotNull, Unique]
        public string UserName { get; set; } = string.Empty;

        [NotNull, Unique]
        public string Email { get; set; } = string.Empty;

        [NotNull]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
