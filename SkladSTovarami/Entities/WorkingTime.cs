using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class WorkingTime
    {
        public int Id { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public double time { get { TimeSpan rez; rez = TimeEnd - TimeStart; return rez.TotalMinutes; } }
        public int? EmployeesId { get; set; }
        public virtual Employee Employees { get; set; }
    }
}
