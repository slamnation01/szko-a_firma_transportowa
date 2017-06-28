using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DomainModel;
using TransportDB;
using FirmaTransportowa.Models;
using Microsoft.AspNet.Identity;
using System.Globalization;

namespace FirmaTransportowa.Controllers
{
    public class RaportsController : Controller
    {
        private TransportDBContext db = new TransportDBContext();
        private ApplicationDbContext identityDb = new ApplicationDbContext();

        // GET: Raports
        public ActionResult Index()
        {
            return View(db.Raports.ToList());
        }

        // GET: Raports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raport raport = db.Raports.Find(id);
            if (raport == null)
            {
                return HttpNotFound();
            }
            return View(raport);
        }

        // GET: Raports/Create
        public ActionResult Create()
        {
            var routesList = db.Routes.OrderBy(u => u.Name).ToList().Select(uu =>
            new SelectListItem { Value = uu.Name.ToString(), Text = uu.Name }).ToList();
            ViewBag.Routes = routesList;

            return View();
        }

        // POST: Raports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PassengersNumber,FuelCost,Distance")] Raport raport, string date, string Trasa)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var driverName = identityDb.Users.Where(x => x.Id == userId).FirstOrDefault().FirstName + " "
                    + identityDb.Users.Where(x => x.Id == userId).FirstOrDefault().LastName;
                DateTime finalDate = DateTime.ParseExact(date, "dd-MM-yyyy H:mm", CultureInfo.InvariantCulture);

                raport.Date = finalDate;
                raport.RouteName = Trasa;
                raport.DriverId = driverName; 
                db.Raports.Add(raport);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(raport);
        }

        // GET: Raports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raport raport = db.Raports.Find(id);
            if (raport == null)
            {
                return HttpNotFound();
            }
            return View(raport);
        }

        // POST: Raports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PassengersNumber,FuelCost,Distance,DriverId")] Raport raport)
        {
            if (ModelState.IsValid)
            {
                db.Entry(raport).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(raport);
        }

        // GET: Raports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raport raport = db.Raports.Find(id);
            if (raport == null)
            {
                return HttpNotFound();
            }
            return View(raport);
        }

        // POST: Raports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Raport raport = db.Raports.Find(id);
            db.Raports.Remove(raport);
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
    }
}
