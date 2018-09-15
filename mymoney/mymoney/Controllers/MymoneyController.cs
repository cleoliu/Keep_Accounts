using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mymoney.Controllers
{
    public class MymoneyController : Controller
    {
        public ActionResult Index() // --Index--
        { 
            List<Models.Daily> result = new List<Models.Daily>(); //回傳計帳列表- GetSet_model_name:Daily
            using (Models.MymoneyEntities db = new Models.MymoneyEntities()) //db
            {
                result = (from s in db.Mymoney select s).ToList();  //LinQ - DB_name:Mymoney
                return View(result);    //result view
            }
        }

        
        public ActionResult Create() // --建立紀錄頁面--
        {
            return View();
        }

        
        [HttpPost]
        public ActionResult Create(Models.Daily postback)   // --Create POST--
        {
            using (Models.MymoneyEntities db = new Models.MymoneyEntities())
            {
                db.Mymoney.Add(postback);   //將回傳資料postback,加入至Mymoney DB
                TempData["ResultMessage"] = String.Format("紀錄[{0}]成功建立", postback.Item);    //設定成功訊息
                db.SaveChanges();   //儲存資料
            }
            return RedirectToAction("Index");
        }


        
        public ActionResult Edit(int id)    // --編輯頁--
        {
            using (Models.MymoneyEntities db = new Models.MymoneyEntities())
            {
                var result = (from s in db.Mymoney where s.Id == id select s).FirstOrDefault(); //抓取Daily.Id,等於輸入id的資料
                if (result != default(Models.Daily)) //判斷此id是否有資料
                {
                    return View(result); //如果有回傳編輯商品頁面
                }
                else  //如果沒有資料則顯示錯誤訊息,導回Index頁
                {   
                    TempData["resultMessage"] = "資料有誤，請重新操作";
                    return RedirectToAction("Index");
                }
            }
        }

        
        [HttpPost]
        public ActionResult Edit(Models.Daily postback) // --編輯頁 POST--
        {
            if (this.ModelState.IsValid) //判斷使用者輸入資料是否正確
            {
                using (Models.MymoneyEntities db = new Models.MymoneyEntities())
                {
                    var result = (from s in db.Mymoney where s.Id == postback.Id select s).FirstOrDefault();    //抓取Mymoney.Id,等於回傳postback.Id的資料
                    //儲存使用者變更資料
                    result.Item = postback.Item;
                    result.Price = postback.Price;
                    result.Payment = postback.Payment;
                    
                    db.SaveChanges();   //儲存
                    TempData["ResultMessage"] = String.Format("紀錄[{0}]成功編輯", postback.Item);    //成功訊息,導回index頁
                    return RedirectToAction("Index");
                }
            }
            else //如果資料不正確,導向自己(Edit頁)
            {
                return View(postback);
            }
        }


        [HttpPost]
        public ActionResult Delete(int id)    // --刪除 POST--
        {
            using (Models.MymoneyEntities db = new Models.MymoneyEntities())
            {
                var result = (from s in db.Mymoney where s.Id == id select s).FirstOrDefault(); //抓取Daily.Id等於輸入id的資料
                if (result != default(Models.Daily)) //判斷id是否有資料
                {
                    db.Mymoney.Remove(result);
                    db.SaveChanges();   //儲存
                    TempData["ResultMessage"] = String.Format("紀錄[{0}]成功刪除", result.Item);    //設定成功訊息,回index頁
                    return RedirectToAction("Index");
                }
                else  //如果沒有資料,顯示錯誤訊息,導回Index頁
                {
                    TempData["resultMessage"] = "資料不存在，請重新操作";
                    return RedirectToAction("Index");
                }
            }
        }
    }
}