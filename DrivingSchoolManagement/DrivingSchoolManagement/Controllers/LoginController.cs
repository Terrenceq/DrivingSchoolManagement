using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;
using System.Text;
using DrivingSchoolManagement.ViewModels;

namespace DrivingSchoolManagement.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CheckProvidedCredentials(LoginViewModel userCredentials)
        {
            using (var db = new DrivingSchoolManagementEntities())
            {
                try
                {
                    var bytes = new UTF8Encoding().GetBytes(userCredentials.Password);
                    var hashedPassword = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);
                    var password = Convert.ToBase64String(hashedPassword);

                    var userCredential = db.UserCredentials.FirstOrDefault(x => x.Login == userCredentials.Login && x.Password == password);

                    if (userCredential != null)
                    {
                        var user = userCredential.UserID;
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Podany login lub has³o s¹ nieprawid³owe." }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception e)
                {
                    return Json(new { success = false, responseText = "Podany login lub has³o s¹ nieprawid³owe." }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}