using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shipping.API.Model
{
    public class Pickup
    {
        public DateTime MinDate { get; internal set; }
        public DateTime MaxDate { get; internal set; }
        public string ShipperId { get; internal set; }
    }
}
