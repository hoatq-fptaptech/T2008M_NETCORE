using System;
using System.Collections.Generic;

#nullable disable

namespace T2008M_NetCoreApi.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
