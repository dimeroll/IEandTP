using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class ItemCategory
    {
        public ItemCategory()
        {
            ShopItems = new List<ShopItem>();
        }

        public int ItemCategoryId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Категорія товару")]
        public string ItemCategoryName { get; set; }

        public virtual ICollection<ShopItem> ShopItems { get; set; }
    }
}
