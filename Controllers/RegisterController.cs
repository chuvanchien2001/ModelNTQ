using ModelNTQ.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace ModelNTQ.Controllers
{
    public class RegisterController : Controller
    {
        private ModelNTQDB db = new ModelNTQDB();
        // GET: Register
        public ActionResult Register()
        {

            return View();
        }

        //POST: REGISTER
       [HttpPost]
       [ValidateAntiForgeryToken]
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

        // create a string MD5
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