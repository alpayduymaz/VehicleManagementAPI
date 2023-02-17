using BLL.EntityCore.Abstract;
using DAL.Core.Entity;
using DAL.Core.Entity.Concrete;
using Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.EntityCore.Concrete
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(EntityContext context) : base(context)
        {

        }
    }
}
