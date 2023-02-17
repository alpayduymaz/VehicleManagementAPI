using DAL.Core.Entity.Abstract;
using Entity.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Abstract
{
    public interface ICarRepository : IEntityBaseRepository<Car>
    {
    }
}
