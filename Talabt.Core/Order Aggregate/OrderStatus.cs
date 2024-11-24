using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Talabt.Core.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="pending")]
        pending,
        [EnumMember(Value ="Payment Recived")]
        PaymentRecived,
        [EnumMember(Value ="Payment Failed")]
        PaymentFailed
    }
}
