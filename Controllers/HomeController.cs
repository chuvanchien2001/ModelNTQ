using ModelNTQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace ModelNTQ.Controllers
{
    public class HomeController : Controller
    {
        private ModelNTQDB db = new ModelNTQDB();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DisplaySuplier(int? page)
        {
            var supliers = db.Products.Select(c => c);
            supliers = supliers.OrderBy(s => s.Id);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(supliers.ToPagedList(pageNumber,pageSize));
        }


    }
}