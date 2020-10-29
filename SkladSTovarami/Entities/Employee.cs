using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Display(Name = "Login")]
        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public virtual ICollection<Check> Checks { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<DeliveryNote> DeliveryNotes { get; set; }        

        public Employee()
        {
            Checks = new List<Check>();
            Orders = new List<Order>();
            DeliveryNotes = new List<DeliveryNote>();            
        }
    }
}
