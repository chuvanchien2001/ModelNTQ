using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ModelNTQ.Models;
using PagedList;

namespace ModelNTQ.Controllers
{
    public class ProductsController : Controller
    {
        private ModelNTQDB db = new ModelNTQDB();
        //private List<Product> products = new List<Product>();
        // GET: Products
        public ActionResult Index(string sortOrder,string searchString,bool? trending,string currentFilter,int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.sortByViews = sortOrder == "NumberViews" ? "NumberViews_desc" : "NumberViews";
            
            if(searchString!=null)
            {
                page = 1;

            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var products = db.Products.Select(p => p);
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Slug.Contains(searchString));
            }
            if (trending.HasValue && trending.Value)
            {
                products = products.Where(p => p.Trending);
            }
            
            switch (sortOrder)
            {
                case "NumberViews":
                    products = products.OrderBy(s => s.NumberViews);
                    break;
                case "NumberViews_desc":
                    products = products.OrderByDescending(s => s.NumberViews);
                    break;
                default:
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber,pageSize)); 
           
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryID,ShopID,Slug,Detail,Trending,Status,NumberViews,Price,CreatedAt,UpdatedAt,DeletedAt")] Product product)
        {
            product.CreatedAt = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    product.Image = "";
                    var f = Request.Files["ImageFile"];
                    if(f!=null && f.ContentLength>0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Images/Image/" + FileName);
                        f.SaveAs(UploadPath);
                        product.Image = FileName;
                    }    
                    db.Products.Add(product);
                    db.SaveChanges(); 
                }
                return RedirectToAction("Index");

            }
            catch(Exception ex)
            {
                ViewBag.Error = "Lỗi nhập dữ liệu" + ex.Message;
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                return View(product);
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryID,ShopID,Slug,Detail,Trending,Status,NumberViews,Price,CreatedAt,UpdatedAt,DeletedAt")] Product product)
        {
            product.UpdatedAt = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    product.Image = "";
                    var f = Request.Files["ImageFile"];
                    if (f != null && f.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(f.FileName);
                        string UploadPath = Server.MapPath("~/Images/Image/" + FileName);
                        f.SaveAs(UploadPath);
                        product.Image = FileName;
                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Lỗi sửa dữ liệu" + ex.Message;
                ViewBag.CategoryID = new SelectList(db.Products, "CategoryID", "CategoryName", product.CategoryID);
                return View(product);
            }
        }
        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            product.CreatedAt = DateTime.Now;
            try
            {
                
                db.Products.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {

                ViewBag.Error = "Không xóa được danh mục này" + ex.Message;
                return View("Delete", product);
            }
        
 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //public ActionResult Trending()
        //{
        //    var model =products.Where(p =>p.Trending ).ToList();    
        //    return View(model);
        //}
        //public ActionResult AddToTrending(int id)
        //{
        //    var product = products.FirstOrDefault(p => p.Id == id);
        //    if (product != null)
        //    {
        //        product.Trending = true;
        //    }
        //    return RedirectToAction("Trending");
        //}
    }
}
