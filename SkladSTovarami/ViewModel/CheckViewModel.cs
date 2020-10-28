using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.ViewModel
{
    class CheckViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double Sum { get { return Price * Count; } }
        public int GoodId { get; set; }  
    }
}
