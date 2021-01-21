using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraBlog.Models
{
    public class Document
    {
        //public int Id { get; set; }

        public string name { get; set; } // name property is unique and can be used as ID

        public bool isArchived { get; set; }
		
		public string[] Paragraphs { get; set; }
		
		public string[] Pictures { get; set; }

        public string CreatedBy { get; set; }

        public DateTime Created { get; set; }
    }
}
