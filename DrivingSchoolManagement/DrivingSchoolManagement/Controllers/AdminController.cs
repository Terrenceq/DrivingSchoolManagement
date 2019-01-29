using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Web.Mvc;

namespace DrivingSchoolManagement.Controllers
{
    public class AdminController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();
        [DSMAuthorize("Manager")]
        public ActionResult Index()
        {
            var stats = new AdminPanelViewModel(db);
            return View(stats);
        }

        [DSMAuthorize("Manager")]
        public ActionResult Drivers()
        {
            return View();
        }

        [DSMAuthorize("Manager")]
        public ActionResult Managers()
        {
            return View();
        }
    }
}