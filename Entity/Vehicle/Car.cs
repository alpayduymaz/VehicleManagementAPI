using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Vehicle
{
    public class Car:BaseEntity
    {
        public int Id { get; set; }
        public string Color { get; set; }
    }
}
