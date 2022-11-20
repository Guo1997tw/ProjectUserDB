using ProjectUserDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        public ActionResult UserTableList()
        {
            IQueryable<UserTable> UserTableListAll = from _UserTable in _MUDBC.UserTables select _UserTable;

            if(UserTableListAll == null) { return HttpNotFound(); }
            else { return View(UserTableListAll); }
        }
        [HttpGet]
        public ActionResult UserTableDetails(int? _ID)
        {
            if (_ID == null) { return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); }

            IQueryable<UserTable> UserTableDetailsOne = from _UserTable in _MUDBC.UserTables
                                                        where _UserTable.UserId == _ID
                                                        select _UserTable;
            
            if (UserTableDetailsOne == null) { return HttpNotFound(); }
            else { return View(UserTableDetailsOne.FirstOrDefault()); }
        }
    }
}