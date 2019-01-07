using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;
using System.Text;

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
                    var bytes = new UTF8Encoding().GetBytes(item.Password);
                    var hashedPassword = System.Security.Cryptography.MD5.Create().ComputeHash(bytes);

                    var password = Convert.ToBase64String(hashedPassword);

                    var user = db.UserCredentials.FirstOrDefault(x => x.Login == item.Login && x.Password == password);

                    if (user != null)
                    {
                        Session["UserType"] = user.UserType.UserTypeName;
                        Session["UserCredentialID"] = user.UserCredentialID;

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