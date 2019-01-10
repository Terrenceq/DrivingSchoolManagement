using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using System.Web.Security;
using DrivingSchoolManagement.Models;
using System.Text;
using DrivingSchoolManagement.ViewModels;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace DrivingSchoolManagement.Controllers
{
    public class LoginController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        public ActionResult Index()
        {
            if (Session["UserID"] != null)
            {
                var userId = (int)Session["UserID"];
                var user = db.Users.FirstOrDefault(x => x.UserID == userId);

                if (user != null)
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CheckProvidedCredentials(LoginViewModel userCredentials)
        {
            try
            {
                var password = DrivingSchoolDataProvider.EncryptPassword(userCredentials.Password);

                var userCredential = db.UserCredentials.FirstOrDefault(x => x.Login == userCredentials.Login && x.Password == password);

                if (userCredential != null)
                {
                    Session["UserID"] = userCredential.UserID;
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
                var providedLoginUser = db.UserCredentials.FirstOrDefault(x => x.Login == providedText.Input);
                var providedEmailUser = db.Users.FirstOrDefault(x => x.Email == providedText.Input);

                if (providedLoginUser != null)
                {
                    var password = DrivingSchoolDataProvider.DecryptPassword(providedLoginUser.Password);
                    var email = db.Users.FirstOrDefault(x => x.UserID == providedLoginUser.UserID).Email;
                    SendEmail(email, password);

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

        public void SendEmail(string email, string password)
        {
            var fromAddress = new MailAddress("piotrek.olesko@gmail.com", "Osrodek Szkolenia Kierowcow");
            var toAddress = new MailAddress(email);
            const string fromPassword = "wiselka12";
            const string subject = "Haslo do strefy kursanta OSK";
            string body = "Twoje haslo dostepu do serwisu OSK to: " + password;

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
        }  
    }
}