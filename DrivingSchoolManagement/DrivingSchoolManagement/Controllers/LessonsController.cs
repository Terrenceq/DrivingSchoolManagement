using DrivingSchoolManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrivingSchoolManagement.Controllers
{
    public class LessonsController : Controller
    {
        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SaveLessonDateInSession(string lessonDate)
        {
            Session["LessonDate"] = DateTime.Parse(lessonDate).ToShortDateString();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}