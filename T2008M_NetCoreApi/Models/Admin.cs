﻿using System;
using System.Collections.Generic;

#nullable disable

namespace T2008M_NetCoreApi.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
