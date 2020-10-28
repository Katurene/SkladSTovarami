using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkladSTovarami.Entities;

namespace SkladSTovarami.ViewModel
{
    class DeliveryViewModel
    {
        [Display(Name = "№")]
        public int Id { get; set; }

        [Display(Name = "№ товара")]
        public int ProductsId { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Количество")]
        public int Count { get; set; }

        [Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Сумма")]
        public double Summa { get { return Count * Price; } }

        public DeliveryViewModel(DeliveryInfo lst, int id)
        {
            this.Count = lst.Count;
            Price = lst.Price;
            ProductsId = lst.Products.Id;
            if (lst.Products.Secondary == null)
            {
                Name = lst.Products.MainProduct.CodeName;
            }
            else
            {
                Name = lst.Products.Secondary.Name;
            }
            Id = id;
        }
        public DeliveryViewModel()
        {

        }
    }
}
