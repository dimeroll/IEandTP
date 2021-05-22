using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class ShopWorker
    {
        public int ShopWorkerId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я працівника")]
        public string Name { get; set; }
        public int RoleId { get; set; }
        public int ShopId { get; set; }

        public virtual Role Role { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
