using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2_IEandTP.Models
{
    public class Customer
    {
        public Customer()
        {
            Purchases = new List<Purchase>();
        }

        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Ім'я клієнта")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Display(Name = "Дата народження клієнта")]
        public DateTime DateOfBirth { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
