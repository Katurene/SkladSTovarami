using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class OrderInfo
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int? ProductsId { get; set; }
        public virtual Product Products { get; set; }
        public int Count { get; set; }
    }
}
