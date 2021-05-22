using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class Shop
    {
        public Shop()
        {
            ShopWorkers = new List<ShopWorker>();

            ShopItems = new List<ShopItem>();
        }

        public int ShopId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Назва магазину")]
        public string ShopName { get; set; }

        public virtual ICollection<ShopItem> ShopItems { get; set; }
        public virtual ICollection<ShopWorker> ShopWorkers { get; set; }
    }
}
