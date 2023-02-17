using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Vehicle
{
    public class Vehicle:BaseEntity
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public int CreatedYear { get; set; }
    }
}
