﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CategoryDTO:CommonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
