using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Models
{
    [Table("Categories")]
    public class Category : BaseModel
    {
        public string Name { get; set; }
        public int ParentID { get; set; }
        [ForeignKey("ParentID")]
        public virtual Category Parent { get; set; }
    }
}
