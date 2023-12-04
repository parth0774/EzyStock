using EzyStock.DataAccess;
using EzyStock.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EzyStock.Controllers
{
    public class HomeController : Controller
    {
        private readonly DB_Context _context;

        public HomeController(DB_Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginInfo userlogin)
        {
            List<LoginInfo> login = _context.Logins.ToList();
            LoginInfo user = _context.Logins.Where(u => u.UserName == userlogin.UserName && u.Password == userlogin.Password).FirstOrDefault();
            if (user!=null)
            {
                if(user.UserName=="admin" && user.Password=="admin")
                {
                    return View("CreateAccounts");
                }
                else if (user.AccType==AccountType.Supplier)
                {
                    return RedirectToAction("GetSupplier","Suppliers");
                }
                else if (user.AccType==AccountType.User)
                {
                    return RedirectToAction("Index", "Inventory");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View("InvalidLogin");
            }


        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}