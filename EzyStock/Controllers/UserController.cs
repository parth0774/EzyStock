using Microsoft.AspNetCore.Mvc;
using EzyStock.Models;
using System.Linq;
using EzyStock.DataAccess;

namespace EzyStock.Controllers
{
    public class UserController : Controller
    {
        private readonly DB_Context _context; // Replace YourDbContext with your actual DbContext

        public UserController(DB_Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username, string password, User user)
        {
            if (ModelState.IsValid)
            {
                LoginInfo loginInfo = new LoginInfo();
                loginInfo.Password = password;
                loginInfo.UserName = username;
                loginInfo.AccType = AccountType.User;
                _context.Add(loginInfo);

                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}