using EzyStock.DataAccess;
using EzyStock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace EzyStock.Controllers
{
    public class OrderController : Controller
    {
        private readonly DB_Context _context;
        public OrderController(DB_Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> Create()
        {
            List<Supplier> suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);
        }

        public  IActionResult CreateProducts(int id)
        {
            Supplier supplier =   _context.Suppliers.Include(s => s.Products).FirstOrDefault(s => s.Id == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order ,List<int> productIds, List<int> quantities, int supplier)
        {
            if (ModelState.IsValid)
            {
 
                order.Status = OrderStatus.Requested;
                order.SupplierID = supplier;
                await _context.AddAsync(order);
                await _context.SaveChangesAsync();
                List<int> ProductId = productIds.ToList();
                List<int> Quantity = quantities.ToList();
                for (var i = 0; i < ProductId.Count; i++)
                {
                    var product = await _context.Products.FindAsync(ProductId[i]);
                    if (product != null)
                    {
                        if (Quantity[i]>=1)
                        {
                            OrderProduct orderProduct = new OrderProduct();
                            orderProduct.Product=product;
                            orderProduct.ProductId = ProductId[i];
                            orderProduct.Quantity = Quantity[i];
                            orderProduct.OrderId = order.Id;
                            await _context.AddAsync(orderProduct);
                            await _context.SaveChangesAsync();
                        }

                    }
                }
                return RedirectToAction(nameof(ViewOrder), new { id = order.Id });
            }
            List<Supplier> suppliers = await _context.Suppliers.ToListAsync();
            return View("Create", suppliers);
        }

        public async Task<IActionResult> ViewOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            List<OrderProduct> orderProduct = await _context.OrderProducts.Where(o => o.OrderId == id).Include(o=>o.Product).ToListAsync();
            ViewBag.OrderProduct = orderProduct;
            ViewBag.Supplier = _context.Suppliers.FindAsync(order.SupplierID);
            return View(order);
        }
        public async Task<IActionResult> ViewOrdersUser()
        {

            List<Order> orders = await _context.Orders.Include(o=>o.Supplier).ToListAsync();
            return View(orders);

        }
        public  async Task<IActionResult> ViewOrdersSupplier(int id)
        {
            List<Order> orders = await _context.Orders.Where(o=> o.SupplierID==id).Include(o => o.Supplier).ToListAsync();
            return View(orders);
        }
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order != null)
            {
                order.Status = OrderStatus.Approved;
                Invoice invoice = new Invoice();
                invoice.Status=InvoiceStatus.UnPaid;
                invoice.Date =DateOnly.FromDateTime(DateTime.Now);
                invoice.OrderId=order.Id;
                _context.Add(invoice);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewOrdersSupplier", new { id = order?.SupplierID});
        }
        public IActionResult CancelRequest(int id)
        {
            var order = _context.Orders.Find(id);

            if (order != null)
            {
                order.Status = OrderStatus.Cancelled;

                _context.SaveChanges();
            }
            return RedirectToAction("ViewOrdersSupplier", new { id = order?.SupplierID });
        }
    }
}
