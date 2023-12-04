using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations.Schema;

namespace EzyStock.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public DateOnly LastOrderDate { get; set; }
        public int LastOrderQuantity { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; } 
        public virtual Product? Product { get; set; }

        //low  inventory intecator in view section 

    }
}
