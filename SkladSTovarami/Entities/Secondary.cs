using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class Secondary
    {
        [Key]
        [ForeignKey("Product")]
        public int Id { get; set; }
        public string Type { get; set; } //шлем
        public string Name { get; set; } //детский
        public string Characteristics { get; set; }//велосипедный
        public virtual Product Product { get; set; }

    }
}
