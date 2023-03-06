using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        [HttpPost]
        public ActionResult Register(User username)
        {
            if (ModelState.IsValid)
            {
                var check = db.Users.FirstOrDefault(s => s.Email == username.Email);
                if (check == null)
                {
                    username.Password = GETMD5(username.Password);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Users.Add(username);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.Error = "Email already exists";
                    return RedirectToAction("Index", "Login");
                }

            }
            return RedirectToAction("Index", "Login");
        }
        public static string GETMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
    
}