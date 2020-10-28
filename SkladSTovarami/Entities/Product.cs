using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int Balance { get; set; }
        public double PricePurchase { get; set; } //цена покупки
        public double SellPrice { get; set; } //цена продажи

        public virtual MainProduct MainProduct { get; set; }
        public virtual Secondary Secondary { get; set; }

        public virtual ICollection<CheckInfo> CheckInfos { get; set; }
        public virtual ICollection<OrderInfo> OrderInfos { get; set; }
        public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; }

        public Product()
        {
            CheckInfos = new List<CheckInfo>();
            OrderInfos = new List<OrderInfo>();
            DeliveryInfos = new List<DeliveryInfo>();
        }
    }
}
