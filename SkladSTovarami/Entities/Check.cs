using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Check
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Sum { get; set; }
        public int? EmployeesId { get; set; }
        public virtual Employee Employees { get; set; }
        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CheckInfo> CheckInfos { get; set; }

        public Check()
        {
            CheckInfos = new List<CheckInfo>();
        }       
    }
}
