using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using FirmaTransportowa.DAL;
using DayPilot.Web.Mvc.Json;

namespace FirmaTransportowa.Controllers
{
    public class CalendarController : Controller
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();

        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }

        class Dpc : DayPilotCalendar
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            protected override void OnInit(InitArgs e)
            {
                UpdateWithMessage("Welcome!", CallBackUpdateType.Full);
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                DataIdField = "Id";
                DataStartField = "Start";
                DataEndField = "End";
                DataTextField = "Text";

                Events = from e in dc.CalendarEvents where !((e.End <= VisibleStart) || (e.Start >= VisibleEnd)) select e;
            }
        }

        

        public ActionResult Edit(string id)
        {
            var ids = Convert.ToInt32(id);
            var t = (from tr in dc.CalendarEvents where tr.Id == ids select tr).First();
            var ev = new EventData
            {
                Id = t.Id,
                Start = t.Start,
                End = t.End,
                Text = t.Text
            };
            return View(ev);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(FormCollection form)
        {
            int id = Convert.ToInt32(form["Id"]);
            DateTime start = Convert.ToDateTime(form["Start"]);
            DateTime end = Convert.ToDateTime(form["End"]);
            string text = form["Text"];

            var record = (from e in dc.CalendarEvents where e.Id == id select e).First();
            record.Start = start;
            record.End = end;
            record.Text = text;
            dc.SubmitChanges();

            return JavaScript(SimpleJsonSerializer.Serialize("OK"));
        }


        public ActionResult Create()
        {
            return View(new EventData
            {
                Start = Convert.ToDateTime(Request.QueryString["start"]),
                End = Convert.ToDateTime(Request.QueryString["end"])
            });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(FormCollection form)
        {
            DateTime start = Convert.ToDateTime(form["Start"]);
            DateTime end = Convert.ToDateTime(form["End"]);
            string text = form["Text"];
            int resource = Convert.ToInt32(form["Resource"]);
            //string recurrence = form["Recurrence"];

            var toBeCreated = new CalendarEvent() { Start = start, End = end, Text = text };
            dc.CalendarEvents.InsertOnSubmit(toBeCreated);
            dc.SubmitChanges();

            return JavaScript(SimpleJsonSerializer.Serialize("OK"));
        }

        public class EventData
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public SelectList Resource { get; set; }
            public string Text { get; set; }
            //public string UserId { get; set; }
        }
    }
}