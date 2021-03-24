using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [Table("Products")]
    public class Product : BaseModel
    {
        public int CategoryID { get; set; }

        public Category Category { get; set; }
    }
}
