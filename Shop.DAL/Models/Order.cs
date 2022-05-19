using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderPlaced { get; set; }
        public DateTime? OrderFulfilled { get; set; }
        
        //Foreign key relationship
        public int CustomerId { get; set; }
        
        //Navigation property: 1 customer per order
        public Customer Customer { get; set; }

        //Navigation property: many products per order
        public ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
