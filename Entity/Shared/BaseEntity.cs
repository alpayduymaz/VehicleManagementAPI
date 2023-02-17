using Entity.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Shared
{
    public class BaseEntity
    {
        /// <summary>
        /// Kaydı oluşturan kişi sicil tekil bilgisidir.
        /// </summary>
        public int? CreatedUserId { get; set; }
        /// <summary>
        /// Kaydı son güncelleyen kişi tekil bilgisidir.
        /// </summary>
        public int? LastUpdatedUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastUpdatedAt { get; set; }
        /// <summary>
        /// Kaydın aktiflik, pasiflik ve dilinme durumlarının tutulduğu alandır.
        public DataStatus DataStatus { get; set; }
        /// <summary>
        /// Kaydı oluştuna kişi bilgilerini döndürür.
        /// Kaydın işlem zamanı bilgisidir.
        /// </summary>
        public User CreatedUser { get; set; }

        /// <summary>
        /// Kaydı son güncelleyen kişi bilgilerini döndürür.
        /// </summary>
        public User LastUpdatedUser { get; set; }
        public DateTime? ProcessingTime { get; set; }
    }

    /// <summary>
    /// Veri işlem durumlarının enum değeridir.
    /// </summary>
    public enum DataStatus
    {
        /// <summary>
        /// Verinin silinme durumudur.
        /// </summary>
        Deleted = 1,

        /// <summary>
        /// Verinin aktiflik durumudur.
        /// </summary>
        Activated,

        /// <summary>
        /// Verinin pasiflik durumudur.
        /// </summary>
        DeActivated
    }
}
