using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class CheckInfo
    {
        public int Id { get; set; }
        public int? CheckId { get; set; }
        public virtual Check Check { get; set; }
        public int? ProductsId { get; set; }
        public virtual Product Products { get; set; }
        public int Count { get; set; }
        public double Sum { get { return Products.SellPrice * Count; } }
    }
}
