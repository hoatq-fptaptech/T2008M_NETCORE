using System;
using System.Collections.Generic;

#nullable disable

namespace T2008M_NetCoreApi.Models
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }

        public virtual Category Category { get; set; }
    }
}
