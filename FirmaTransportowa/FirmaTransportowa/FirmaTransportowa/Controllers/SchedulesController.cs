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

namespace FirmaTransportowa.Controllers
{
    public class SchedulesController : Controller
    {
        private TransportDBContext db = new TransportDBContext();
        private ApplicationDbContext identityDb = new ApplicationDbContext();

        // GET: Schedules
        public ActionResult Index()
        {
            return View(db.Schedules.ToList());
        }

        // GET: Schedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // GET: Schedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                //var _user = identityDb.Users.FirstOrDefault(x => x.Id.Equals(userId));

                schedule.UserId = userId;

                db.Schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(schedule);
        }

        // GET: Schedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday")] Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schedule);
        }

        // GET: Schedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Schedule schedule = db.Schedules.Find(id);
            db.Schedules.Remove(schedule);
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

        public ActionResult ChooseDriver()
        {
            var roleID = identityDb.Roles.Where(x => x.Name == "Driver").FirstOrDefault().Id;
            var usersInRole = identityDb.Users.Where(m => m.Roles.Any(r => r.RoleId == roleID)).ToList().
                Select(x => new SelectListItem { Value = x.Id, Text = x.FirstName + " " + x.LastName }).ToList();

            ViewBag.Driver = usersInRole;

            return View();
        }

        public ActionResult DriverDetail(string Kierowcy)
        {
            var schedule = db.Schedules.Where(x => x.UserId == Kierowcy).LastOrDefault();
            var user = identityDb.Users.Where(x => x.Id == Kierowcy).Select(x => x.FirstName) + " "
                + identityDb.Users.Where(x => x.Id == Kierowcy).Select(x => x.LastName);

            ViewBag.User = user;

            return View(schedule);
        }

        public ActionResult Back()
        {
            return RedirectToAction("ChooseDriver");
        }
    }
}
