using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraBlog.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }

        public ICollection<string> Users { get; set; }

        public ICollection<string> Categories { get; set; }
    }
}
