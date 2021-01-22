using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraBlog.DTOs
{
    public class SimpleNewsFeedDTO
    {
        public string Name { get; set; }

        public string Creator { get; set; }

        public string[] Pictures { get; set; }

        public string[] Paragraphs { get; set; }

        public int Interest { get; set; }
    }
}
