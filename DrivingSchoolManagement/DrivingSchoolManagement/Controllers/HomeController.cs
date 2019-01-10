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
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";

            return View();
        }

        public ActionResult HomeCardPartial()
        {
            var result = new DrivingSchoolManagement.ViewModels.HomeCarouselViewModel();
            result.OurCategories = db.Categories.ToList().Distinct().Take(3).ToList();
            result.OurDrivers = db.Users.Where(w => w.PermissionLevelID == 200).ToList().Distinct().Take(3).ToList();
            result.OurManagers = db.Users.Where(w => w.PermissionLevelID == 300).ToList().Distinct().Take(3).ToList();
            result.OurVehicles = db.Vehicles.ToList().Distinct().Take(3).ToList();
           
            return PartialView("_homeCardAboutPartial", result);
        }

        public ActionResult Logout()
        {
            Session["UserID"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}

public enum HeaderViewRenderMode { Full, Title }