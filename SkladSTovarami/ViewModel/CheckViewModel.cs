using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkladSTovarami.Entities;

namespace SkladSTovarami.ViewModel
{
    public class CheckViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double Sum { get { return Price * Count; } }
        public int GoodId { get; set; }

        //public CheckViewModel(CheckInfo checks, int id) //А это здесь надо??????
        //{
        //    this.Count = checks.Count;
        //    Price = checks.Products.PricePurchase;
        //    GoodId = checks.Products.Id;
        //    if (checks.Products.Secondary == null)
        //    {
        //        Name = checks.Products.MainProduct.CodeName;
        //    }
        //    else
        //    {
        //        Name = checks.Products.Secondary.Name;
        //    }
        //    Id = id;
        //}

        public CheckViewModel()
        {

        }
    }
}
