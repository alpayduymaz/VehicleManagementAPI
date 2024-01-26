using Entity.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Vehicle
{
    public class Categories : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
