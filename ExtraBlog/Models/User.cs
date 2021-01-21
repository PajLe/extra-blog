using System.Collections.Generic;

namespace ExtraBlog.Models
{
    public class User
    {
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public ICollection<string> Users { get; set; }

        public ICollection<string> Categories { get; set; }
    }
}
