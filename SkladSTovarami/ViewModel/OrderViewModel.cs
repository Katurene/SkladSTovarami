using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkladSTovarami.Entities;

namespace SkladSTovarami.ViewModel
{
    class OrderViewModel
    {
        public int Id { get; set; }
        public int ProductsId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }

        public OrderViewModel(OrderInfo lst, int id)
        {
            this.Count = lst.Count;
            Price = lst.Products.PricePurchase;
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
        public OrderViewModel()
        {

        }
    }
}
