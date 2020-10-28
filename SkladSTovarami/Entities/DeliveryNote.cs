using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class DeliveryNote
    {
        public int Id { get; set; }
        public double Sum { get; set; }
        public string TypePayment { get; set; }
        public DateTime Date { get; set; }
        public int? EmployeesId { get; set; }
        public virtual Employee Employees { get; set; }

        public virtual ICollection<DeliveryInfo> DeliveryInfos { get; set; }

        public DeliveryNote()
        {
            DeliveryInfos = new List<DeliveryInfo>();
        }        
    }
}
