using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class DeliveryInfo
    {
        public int Id { get; set; }
        public int? DeliveryNoteId { get; set; }
        public virtual DeliveryNote DeliveryNote { get; set; }
        public int? ProductsId { get; set; }//GoodsId
        public virtual Product Products { get; set; }//Goods
        public int Count { get; set; }
        public double Price { get; set; }
        public double Summa { get { return Count * Price; } }
    }
}
