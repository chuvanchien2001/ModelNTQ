using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
            if (user == null)
            {
                ViewBag.errLogin = "Sai tên đăng nhập hoặc mật khẩu";
                return View("Login");
            }
            else
            {
                Session["user"] = user;
                return RedirectToAction("MyProfile","MyProfile");
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["UserName"] = null;
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public ActionResult Register(FormCollection f)
        {
           
                User user = new User();
                string pass = f["password"];
                user.Id = db.Users.Max(u => u.Id) + 1;
                user.UserName = f["username"];
                user.Email = f["email"];
                user.Password = pass;
                user.Role = 0;
                user.Status = 0;
                user.CreatedAt = DateTime.Now;
                db.Users.Add(user);
                db.SaveChanges();
 
            return View("Login");

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