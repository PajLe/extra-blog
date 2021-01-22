using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraBlog.DTOs
{
    public class DocumentDTO
    {
        //public int Id { get; set; }

        public string name { get; set; }

        public string[] Paragraphs { get; set; }

        public string[] Pictures { get; set; }

        public string[] Categories { get; set; }

        public string CreatedBy { get; set; }
    }
}
