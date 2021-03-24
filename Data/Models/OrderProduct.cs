﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [Table("OrderProducts")]
    public class OrderProduct : BaseModel
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }

        [Column(TypeName = "decimal(19,4)")]
        public Decimal Price { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
