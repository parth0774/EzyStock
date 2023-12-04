using EzyStock.DataAccess;
using EzyStock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzyStock.Controllers
{
    public class InvoiceController : Controller
    {

        private readonly DB_Context _context;
        public InvoiceController(DB_Context context)
        {
            _context = context;
        }
        public  IActionResult ViewInvoiceUser()
        {

            List<Invoice> invoices =  _context.Invoices.ToList();
            return View(invoices);

        }
        public async Task<IActionResult> ViewInvoiceSupplier(int id)
        {
            List<int> ordersids = await _context.Orders.Where(o => o.SupplierID==id).Select(o=>o.Id).ToListAsync();
            List<Invoice> invoices = await _context.Invoices.Where(i => ordersids.Contains(i.OrderId)).ToListAsync();
            return View(invoices);
        }
        public IActionResult PaidInvoice(int id)
        {
            Invoice? invoice = _context.Invoices.Find(id);
            if (invoice != null)
            {
                invoice.Status =InvoiceStatus.Paid;
                List<OrderProduct> products = _context.OrderProducts.Where(op => op.OrderId==invoice.OrderId).ToList();
                for (int i = 0; i < products.Count; i++)
                {
                    if (_context.Inventory.Select(p => p.ProductId).Contains(products[i].ProductId))
                    {
                        var inventory = _context.Inventory.FirstOrDefault(iv => iv.ProductId==products[i].ProductId);
                        inventory.Quantity+=products[i].Quantity;
                        inventory.LastOrderDate=invoice.Date;
                        inventory.LastOrderQuantity=products[i].Quantity;
                        _context.Update(inventory);
                        _context.SaveChanges();
                    }
                    else
                    { 
                        var inventory = new Inventory();
                        inventory.ProductId=products[i].ProductId;
                        inventory.Quantity=products[i].Quantity; 
                        inventory.LastOrderDate=invoice.Date;
                        inventory.LastOrderQuantity = products[i].Quantity;
                        _context.Add(inventory);
                        _context.SaveChanges();
                    }
                }  
            }
            return RedirectToAction("ViewInvoiceSupplier", new { id = _context?.Orders?.Find(invoice?.OrderId)?.SupplierID});
        }
    }
}
