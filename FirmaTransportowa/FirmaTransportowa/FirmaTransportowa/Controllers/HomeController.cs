using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TransportDB;
using System.Data.Entity;

namespace FirmaTransportowa.Controllers
{
    public class HomeController : Controller
    {
        private TransportDBContext db = new TransportDBContext();

        public ActionResult Index()
        {
            try
            {
                var _routes = db.Routes.Include(x => x.Stops).Include(x => x.DepartDates).ToList();
                return View(_routes);
            }
            catch (Exception e)
            {
                return View(e.Message.ToString());
            }            
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult LoyalityProgram()
        {
            ViewBag.ClientPoints = 0;

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}