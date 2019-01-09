using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;
using DrivingSchoolManagement.Services;

namespace DrivingSchoolManagement.Controllers
{
    public class HomeController : Controller
    {
        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";

            return View();
        }
    }
}

public enum HeaderViewRenderMode { Full, Title }