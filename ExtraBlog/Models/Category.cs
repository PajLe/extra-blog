﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtraBlog.Models
{
    public class Category
    {
        //public int Id { get; set; }

        public string name { get; set; } // name property is unique and can be used as ID

        public bool isArchived { get; set; }
    }
}
