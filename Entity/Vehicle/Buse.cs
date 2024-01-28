using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Vehicle
{
    public class Buse : BaseEntity
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public bool Headlight { get; set; }
        public Color Color { get; set; }
        public int CategoryId { get; set; }
        public Categories Category { get; set; }
    }
}
