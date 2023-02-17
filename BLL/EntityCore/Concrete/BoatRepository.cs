using BLL.EntityCore.Abstract;
using DAL.Core.Entity;
using DAL.Core.Entity.Concrete;
using Entity.Vehicle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Concrete
{
    public class BoatRepository : EntityBaseRepository<Boat>, IBoatRepository
    {
        public BoatRepository(EntityContext context) : base(context)
        {

        }
    }
}
