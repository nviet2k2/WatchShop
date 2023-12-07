﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class TestModel:CommonModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Email { get; set; }
        public string? Img { get; set; }
    }
}
