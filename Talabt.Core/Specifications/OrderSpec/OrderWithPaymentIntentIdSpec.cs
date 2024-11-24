using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabt.Core.Order_Aggregate;

namespace Talabt.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentIdSpec:BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentIdSpec(string PaymentIntentId):base(O=>O.PaymentIntendId ==PaymentIntentId)
        { }
    }
}
