using Entity.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Users
{
    /// <summary>
    /// Tüm kullanıcıların kaydının tutulduğu tablodur.
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// Sınıf tekil bilgisidir.
        /// </summary>
        public int Id { get; set; }

        //TODO: sicile baklanacak
        /// <summary>
        /// Sicil tekil bilgisidir.
        /// </summary>
        public int? SicilId { get; set; }
    }
}
