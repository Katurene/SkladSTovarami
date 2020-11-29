using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
        public int? CustomerId { get; set; }  //Это здесь не надо
        public virtual Customer Customer { get; set; }  //Убрать
        public int? EmployeesId { get; set; }
        public virtual Employee Employees { get; set; }

        public ICollection<OrderInfo> OrderInfos { get; set; }

        public Order()
        {
            OrderInfos = new List<OrderInfo>();
        }

        public string textValue //для замены true/false на да/нет
        {
            get { return Status == true ? "Выполнена" : "Не выполнена"; }
        }
    }
}
