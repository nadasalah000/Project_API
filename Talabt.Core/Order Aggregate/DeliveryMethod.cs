using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Entities;

namespace Talabt.Core.Order_Aggregate
{
    public class DeliveryMethod:BaseEntity
    {
        public DeliveryMethod(string shortName,string description,decimal cost,string deliveryTime)
        {
          ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }
        public DeliveryMethod() { }
        public string ShortName { get; set; }
        public string Description { get; set; }
        public string DeliveryTime { get; set; }
        public decimal Cost { get; set; }
    }
}
