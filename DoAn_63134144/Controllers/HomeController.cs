using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAn_63134144.Models;

namespace DoAn_63134144.Controllers
{
    public class HomeController : Controller
    {
        private DOANQLEntities14 db = new DOANQLEntities14();

        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                Session["Id"] = null;
                Session["Name"] = null;
                Session["Type"] = null;
            }

            return View();
        }

        public object GetDb()
        {
            return db;
        }

        public ActionResult trangchu()
        {
            var data = db.HRS.ToList();
            return View(data);
        }
        public ActionResult trangchuUser()
        {
            var data = db.HRS.ToList();
            return View(data);
        }
        public ActionResult trangchuHR()
        {
            var data = db.HRS.ToList();
            return View(data);
        }
        public ActionResult trangchuAD()
        {
            var data = db.HRS.ToList();
            return View(data);
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult GioiThieuUser()
        {
            return View();
        }
        public ActionResult GioiThieuHR()
        {
            return View();
        }
        public ActionResult GioiThieuAD()
        {
            return View();
        }
        public ActionResult Security(string value)
        {
            string kind = Session["Type"] != null ? Session["Type"].ToString() : null;
            if (kind != value)
            {
                return RedirectToAction("trangchu", "Home");
            }
            return View();
        }

    }
}