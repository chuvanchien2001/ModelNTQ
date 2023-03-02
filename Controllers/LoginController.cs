using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ModelNTQ.Models;

namespace ModelNTQ.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private ModelNTQDB db = new ModelNTQDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login (string username,string password)
        {
            var user = db.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
            if(user==null)
            {
                ViewBag.errLogin = "Sai tên đăng nhập hoặc mật khẩu";
                return View("Login");
            }    
            else
            {
                Session["UserName"] = username;
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
    
}