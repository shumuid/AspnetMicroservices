using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Discount.Grpc.Entities
{
    public record Coupon
    {
        public int Id { get; init; }
        public string ProductName { get; init; }
        public string Description { get; init; }
        public int Amount { get; init; }
    }
}
