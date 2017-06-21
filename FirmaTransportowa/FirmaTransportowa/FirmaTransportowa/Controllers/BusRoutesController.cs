using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DomainModel;
using TransportDB;
using System;

namespace FirmaTransportowa.Controllers
{
    public class BusRoutesController : Controller
    {
        private TransportDBContext db = new TransportDBContext();

        // GET: BusRoutes
        public ActionResult Index()
        {
            return View(db.Routes.ToList());
        }

        // GET: BusRoutes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusRoute busRoute = db.Routes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // GET: BusRoutes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BusRoutes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Price,DepartDate")] BusRoute busRoute)
        {
            if (ModelState.IsValid)
            {
                db.Routes.Add(busRoute);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(busRoute);
        }

        // GET: BusRoutes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusRoute busRoute = db.Routes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // POST: BusRoutes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Price,DepartDate")] BusRoute busRoute)
        {
            if (ModelState.IsValid)
            {
                db.Entry(busRoute).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(busRoute);
        }

        // GET: BusRoutes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BusRoute busRoute = db.Routes.Find(id);
            if (busRoute == null)
            {
                return HttpNotFound();
            }
            return View(busRoute);
        }

        // POST: BusRoutes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BusRoute busRoute = db.Routes.Find(id);
            db.Routes.Remove(busRoute);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBusStop([Bind(Include = "Id,FirstStop,LastStop,Price,BusRoute_Id")] BusStop busStop)
        {


            return View();
        }
    }
}
