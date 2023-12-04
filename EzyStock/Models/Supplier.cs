
using System.ComponentModel.DataAnnotations.Schema;

namespace EzyStock.Models
{
    public class Supplier
    {
        public int Id { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierEmail { get; set; }
        public string? SupplierPhone { get; set; }
        public ICollection<Product>? Products { get; set; }

    }
}
