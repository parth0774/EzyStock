using System.ComponentModel.DataAnnotations.Schema;

namespace EzyStock.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier? Supplier { get; set; }
        public ICollection<OrderProduct>? Products { get; set; }

    }
}
