using Entity.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Vehicle
{
    public class Car:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string ModelName { get; set; }
        public int Year { get; set; }
        public int Kilometer { get; set; }
        public Color Color { get; set; }
    }
    public enum Color
    {
        Red = 1,
        Blue,
        Black,
        White
    }
}
