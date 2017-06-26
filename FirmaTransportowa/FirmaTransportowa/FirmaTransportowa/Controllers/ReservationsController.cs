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
using Microsoft.AspNet.Identity;
using FirmaTransportowa.Models;
using System.Globalization;

namespace FirmaTransportowa.Controllers
{
    public class ReservationsController : Controller
    {
        private TransportDBContext db = new TransportDBContext();
        private ApplicationDbContext identityDb = new ApplicationDbContext();

        // GET: Reservations
        public ActionResult Index()
        {
            return View(db.Reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PassengerName,ClientName,RouteName,Date")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PassengerName,ClientName,RouteName,Date")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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

        public ActionResult Reservation(string name, string hour)
        {
            var userId = User.Identity.GetUserId();
            var user = identityDb.Users.FirstOrDefault(x => x.Id.Equals(userId));

            ViewBag.Name = name;
            ViewBag.Hour = hour;
            ViewBag.Client = (user.FirstName + " " + user.LastName).ToString();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveReservation([Bind(Include = "Id,PassengerName")] Reservation reservation, string name, string hour, string date)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                reservation.ClientID = userId;
                var _user = identityDb.Users.FirstOrDefault(x => x.Id.Equals(userId));
                reservation.ClientName = _user.FirstName + " " + _user.LastName;
                reservation.RouteName = name;

                string _dateHelper = date + " " + hour;
                DateTime _finalDate = DateTime.ParseExact(_dateHelper, "dd-MM-yyyy H:mm", CultureInfo.InvariantCulture);

                reservation.Date = _finalDate;

                //db.Entry(reservation).State = EntityState.Modified;
                db.Reservations.Add(reservation);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home", null);
        }
    }
}
