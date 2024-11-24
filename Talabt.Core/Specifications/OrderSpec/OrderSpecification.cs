using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Order_Aggregate;

namespace Talabt.Core.Specifications.OrderSpec
{
    public class OrderSpecification:BaseSpecifications<Order>
    {
        public OrderSpecification(string email) : base(o => o.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            AddOrderByDescending(O => O.OrderDate);
        }
        public OrderSpecification(string email, int OrderId) : base(o => o.BuyerEmail == email && o.Id == OrderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
        }
    }
}
