using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mymoney.Controllers
{
    public class MymoneyController : Controller
    {
        // GET: Mymoney
        public ActionResult Index()
        {
            //回傳計帳列表- GetSet_model_name:Daily 
            List<Models.Daily> result = new List<Models.Daily>();

            //db
            using (Models.MymoneyEntities db = new Models.MymoneyEntities())
            {
                //LinQ - DB_name:Mymoney
                result = (from s in db.Mymoney select s).ToList();

                //result view
                return View(result);
            }
        }

        //建立商品頁面
        public ActionResult Create()
        {
            return View();
        }

        //建立頁面 - 資料傳回處理
        [HttpPost]
        public ActionResult Create(Models.Daily postback)
        {
            using (Models.MymoneyEntities db = new Models.MymoneyEntities())
            {
                //將回傳資料postback加入至Mymoney DB
                db.Mymoney.Add(postback);

                //儲存資料
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}