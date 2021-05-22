using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ShopItemId { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата та час покупки")]
        public DateTime PurchaseDateTime { get; set; }

        public virtual ShopItem ShopItem { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
