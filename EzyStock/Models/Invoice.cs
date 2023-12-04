using System.ComponentModel.DataAnnotations.Schema;

namespace EzyStock.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public InvoiceStatus? Status {  get; set; }
        [ForeignKey("Order")]
        public int OrderId {  get; set; }
        public virtual Order? Order { get; set; }
    }
}
