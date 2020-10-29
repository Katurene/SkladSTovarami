using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkladSTovarami.Entities
{
    public class MainProduct
    {
        [Key]
        [ForeignKey("Product")]
        public int Id { get; set; }        
        public string CodeName { get; set; } //вел
        public string Type { get; set; } //горный
        public string Brand { get; set; }//марка  Automatic
        public double TireWidth { get; set; }//ширина шины 1,8 2,1 2 Сaliber
        public int Diameter { get; set; }//диаметр колес24 27 29 26  KillRange
        public int SpeedNum { get; set; }//колич скоростей 21 24 Ammunition         
        public bool Used { get; set; }//  Optic
        public string Info { get; set; }

        public virtual Product Product { get; set; }
    }
}
