using ProjectUserDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace ProjectUserDB.Controllers
{
    public class MVC_UserDBController : Controller
    {
        // GET: MVC_UserDB

        /*開啟資料庫連結*/
        private Models.MVC_UserDBContext _MUDBC = new Models.MVC_UserDBContext();

        /*關閉資料庫連結*/
        protected override void Dispose(bool disposing)
        {
            if (disposing) { _MUDBC.Dispose(); }
            base.Dispose(disposing);
        }

        /*主表清單*/
        public ActionResult UserTableList()
        {
            IQueryable<UserTable> UserTableListAll = from _UserTable in _MUDBC.UserTables select _UserTable;

            if(UserTableListAll == null) { return HttpNotFound(); }
            else { return View(UserTableListAll); }
        }
        [HttpGet]
        
        /*資料明細*/
        public ActionResult UserTableDetails(int? _ID)
        {
            if (_ID == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }

            IQueryable<UserTable> UserTableDetailsOne = from _UserTable in _MUDBC.UserTables
                                                        where _UserTable.UserId == _ID
                                                        select _UserTable;
            
            if (UserTableDetailsOne == null) { return HttpNotFound(); }
            else { return View(UserTableDetailsOne.FirstOrDefault()); }
        }
        
        /*新增*/
        public ActionResult UserTableCreate()
        {
            return View();
        }
        [HttpPost, ActionName("UserTableCreate")]
        [ValidateAntiForgeryToken]
        public ActionResult UserTableCreateConfirm(UserTable _UserTable)
        {
            if((_UserTable != null) && (ModelState.IsValid))
            {
                _MUDBC.UserTables.Add(_UserTable);
                _MUDBC.SaveChanges();

                return RedirectToAction("UserTableList");
            }

            return Content("欄位有誤！請返回上一頁並重新填寫。");
        }

        /*修改*/
        public ActionResult UserTableEdit(int? _ID)
        {
            if (_ID == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }

            UserTable _UT = _MUDBC.UserTables.Find(_ID);

            if (_UT == null) { return HttpNotFound(); }
            else { return View(_UT); }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserTableEdit(UserTable _UserTable)
        {
            if (_UserTable == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }
            
            if(ModelState.IsValid)
            {
                _MUDBC.Entry(_UserTable).State = System.Data.Entity.EntityState.Modified;
                _MUDBC.SaveChanges();

                return RedirectToAction("UserTableList");
            }

            return Content("欄位有誤！請返回上一頁並重新填寫。");
        }

        /*刪除*/
        public ActionResult UserTableDelete(int? _ID)
        {
            if (_ID == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }
            
            UserTable _UT = _MUDBC.UserTables.Find(_ID);

            if(_UT == null) { return HttpNotFound(); }
            else { return View(_UT); }
        }
        [HttpPost, ActionName("UserTableDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult UserTableDeleteConfirm(int? _ID)
        {
            if(ModelState.IsValid)
            {
                UserTable _UT = _MUDBC.UserTables.Find(_ID);
                _MUDBC.UserTables.Remove(_UT);
                _MUDBC.SaveChanges();

                return RedirectToAction("UserTableList");
            }
            else
            {
                ModelState.AddModelError("Value1", "錯誤訊息");
                return View();
            }
        }
    }
}