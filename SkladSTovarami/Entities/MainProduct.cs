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
        public string Type { get; set; } //производитель
        public string Brand { get; set; }//марка  
        public double TireWidth { get; set; }//ширина шины 1,8 2,1 2
        public int Diameter { get; set; }//диаметр колес24 27 29 26  
        public int SpeedNum { get; set; }//колич скоростей 21 24   
        public bool Used { get; set; }
        public string Info { get; set; }

        public virtual Product Product { get; set; }

        public string textValue //для замены true/false на да/нет
        {
            get { return Used == true ? "Да" : "Нет"; }
        }
    }
}
