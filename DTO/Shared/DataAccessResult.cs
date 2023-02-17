using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Shared
{
    public class DataAccessResult<T> where T : class
    {
        public bool Result { get; set; }
        public string ResultMessage { get; set; }
        public T Object { get; set; }
    }
}
