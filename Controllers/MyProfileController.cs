using ModelNTQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace ModelNTQ.Controllers
{
    public class MyProfileController : Controller
    {
        // GET: MyProfile
        private ModelNTQDB db = new ModelNTQDB();
        public ActionResult MyProfile()
        {
            if (Session["UserName"] == null)
            {
                return Content("<h1>Bạn cần đăng nhập trước khi vào giao diện</h1><a href='/Login/Login'>Đăng nhập</a>");
            }
            User u = (User)Session["user"];
            return View(u);
        }
        public ActionResult MyProfilePost()
        {
            int Id = ((User)Session["user"]).Id;
            var u = db.Users.FirstOrDefault(_u => _u.Id == Id);
            string name = Request["Username"];
            if (name == null)
            {
                ViewBag.ErrorUserNameVal = "Username không được để trống";
                return View("MyProfile");
            }
            if (u.UserName != name)
            {
                if (db.Users.FirstOrDefault(x => x.UserName == name) == null)
                {
                    u.UserName = name;
                }
                else
                {
                    ViewBag.ErrorUserNameVal = "Username dã tồn tại";
                    return View("MyProfile");
                }
            }

            string pattern = "[^a-zA-Z0-9]"; // biểu thức chính quy: không phải ký tự a-z, A-Z, 0-9
            bool containsSpecialChars = Regex.IsMatch(u.UserName, pattern); // kiểm tra chuỗi có chứa ký tự đặc biệt hay không
            if (containsSpecialChars)
            {
                ViewBag.ErrorUserNameVal = "Chuỗi không được chứa ký tự đặc biệt";
                return View("MyProfile");
            }
            u.UpdatedAt = DateTime.Now;

            string password_new = Request["password_new"];
            if (password_new != null)
            {
                string password_new_comfirm = Request["password_new_comfirm"];
                string passwordPattern = @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^\w\s]).{8,20}$";
                if (!Regex.IsMatch(password_new, passwordPattern))
                {
                    ViewBag.Error = "Password phải từ 8-20 kí tự bao gồm ít nhất 1 kí tự số, 1 kí tự viết hoa và 1 kí tự đặc biệt";
                    return View("MyProfile");
                }
                if (string.IsNullOrEmpty(password_new_comfirm))
                {
                    ViewBag.Error = "Confirm password không được để trống";
                    return View("MyProfile");
                }
                SHA256 sha = SHA256.Create();
                if (password_new != password_new_comfirm)
                {
                    ViewBag.Error = "password và confirm passowrd không khớp";
                    return View("MyProfile");
                }
                byte[] rs = sha.ComputeHash(Encoding.UTF8.GetBytes(password_new));
                password_new = BitConverter.ToString(rs).Replace("-", string.Empty);
                u.Password = password_new;
            }

            db.SaveChanges();
            ViewBag.Success = "Sửa thông tin thành công";
            return View("MyProfile", u);
        }
    }
}