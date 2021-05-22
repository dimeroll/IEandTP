using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class ShopItem
    {
        public ShopItem()
        {
            Purchases = new List<Purchase>();
        }

        public int ShopItemId { get; set; }
        public int ShopId { get; set; }
        public int ItemCategoryId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Кількість одиниць товару")]
        public int Quantity { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual ItemCategory ItemCategory { get; set; }
    }
}
