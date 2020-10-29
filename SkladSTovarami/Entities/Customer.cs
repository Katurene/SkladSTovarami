using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PassportNumber { get; set; }
        public string Contract { get; set; }  
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public virtual ICollection<Check> Checks { get; set; }

        public Customer()
        {
            Checks = new List<Check>();
        }
    }
}
