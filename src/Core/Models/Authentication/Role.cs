﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryApp.src.Core.Models.Authentication
{
    public class Role : BaseAuthentication
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
