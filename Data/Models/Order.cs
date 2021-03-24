using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [Table("Orders")]
    public class Order : BaseModel
    {
        public int CustomerID { get; set; }
        [Column(TypeName = "decimal(19,4)")]
        public Decimal Price { get; set; }

        public List<OrderProduct> OrderProducts { get; set; }
    }
}
