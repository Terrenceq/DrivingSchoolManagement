using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;

namespace DrivingSchoolManagement.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CheckProvidedCredentials(UserCredential item)
        {
            using (var db = new DrivingSchoolManagementEntities())
            {
                try
                {
                    var user = db.UserCredentials.FirstOrDefault(x => x.Login == item.Login && x.Password == item.Password);
                    if (user != null)
                    {
                        Session["UserID"] = 
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Please, correct all errors.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}