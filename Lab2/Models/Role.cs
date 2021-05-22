using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class Role
    {
        public Role()
        {
            ShopWorkers = new List<ShopWorker>();
        }

        public int RoleId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Роль працівника")]
        public string RoleName { get; set; }

        public virtual ICollection<ShopWorker> ShopWorkers { get; set; }
    }
}
