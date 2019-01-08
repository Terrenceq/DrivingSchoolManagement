using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;
using System.Text;
using DrivingSchoolManagement.ViewModels;
using System.Net.Mail;
using System.Net;

namespace DrivingSchoolManagement.Controllers
{
    public class LoginController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CheckProvidedCredentials(LoginViewModel userCredentials)
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

        [HttpPost, ValidateInput(false)]
        public ActionResult SendEmailWithPassword(RemindPasswordViewModel providedText)
        {
            try
            {
                var providedLoginUser = db.Users.FirstOrDefault(x => x.Login == providedText.Input);
                var providedEmailUser = db.Users.FirstOrDefault(x => x.Email == providedText.Input);

                if (providedLoginUser != null)
                {
                    var password = db.Passwords.FirstOrDefault(x => x.UserID == providedLoginUser.UserID).PasswordHash;

                    var fromAddress = new MailAddress("piotrek.olesko@gmail.com", "Osrodek Szkolenia Kierowcow");
                    var toAddress = new MailAddress("peter.olesko@gmail.com", "Hello");
                    const string fromPassword = "wiselka12";
                    const string subject = "Haslo!";
                    string body = password;

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };
                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                else if (providedEmailUser != null)
                {
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