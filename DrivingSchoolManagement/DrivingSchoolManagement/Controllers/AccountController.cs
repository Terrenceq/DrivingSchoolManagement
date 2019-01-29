using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System.Web.Mvc;
using DrivingSchoolManagement.Models;
using System.Data.Entity.Migrations;

namespace DrivingSchoolManagement.Controllers
{
    public class AccountController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();
        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }

        [DSMAuthorize("User")]
        public ActionResult Password()
        {
            return View();
        }

        public ActionResult UserAccountDetailsFormPartial()
        {
            var model = new AccountDetailsViewModel();
            var userId = (int)Session["UserID"];
            var user = db.Users.FirstOrDefault(x => x.UserID == userId);

            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.DateOfBirth = user.DateOfBirth;
            model.PESEL = user.PESEL;
            model.Email = user.Email;
            model.About = user.About;
            model.StreetName = user.Address.StreetName;
            model.Number = user.Address.Number;
            model.City = user.Address.City;
            model.PostalCode = user.Address.PostalCode;
            model.Country = user.Address.Country;
            model.Login = user.UserCredentials.FirstOrDefault(x => x.UserID == userId).Login;

            if (user.PermissionLevelID == 100)
            {
                var assignedDriver = db.Users.FirstOrDefault(x => x.UserID == user.StudentInfo.AssignedDriverID);
                model.AssignedDriverName = assignedDriver.FirstName + " " + assignedDriver.LastName;
                model.CategoryName = user.StudentInfo.Category.CategoryName;
            }

            if (user.PermissionLevelID == 200)
            {
                model.AssignedStudentsCount = db.AssignedStudents.Count(s => s.DriverID == userId);
                model.CategoryName = user.DriverInfo.Category.CategoryName;
            }
            return PartialView("_userAccountDetailsFormPartial", model);
        }

        public ActionResult AssignedStudentsGridPartial()
        {
            var userId = (int)Session["UserID"];

            var model = new List<AssignedStudentViewModel>();
            var students = db.AssignedStudents.Where(x => x.DriverID == userId);

            foreach (var student in students)
            {
                model.Add(new AssignedStudentViewModel()
                {
                    UserID = student.StudentID,
                    FirstName = student.StudentInfo.User.FirstName,
                    LastName = student.StudentInfo.User.LastName,
                    Email = student.StudentInfo.User.Email,
                    DateOfBirth = student.StudentInfo.User.DateOfBirth
                });
            }
            return PartialView("_assignedStudentsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CheckProvidedPasswordData(UserPasswordChangeViewModel passwords)
        {
            try
            {
                var encryptedPassword = DrivingSchoolDataProvider.EncryptPassword(passwords.OldPassword);
                var userId = (int)Session["UserID"];

                var userCredential = db.UserCredentials.FirstOrDefault(x => x.UserID == userId && x.Password == encryptedPassword);

                if (userCredential != null)
                {
                    userCredential.Password = DrivingSchoolDataProvider.EncryptPassword(passwords.NewPassword);
                    db.UserCredentials.AddOrUpdate(userCredential);
                    db.SaveChanges();

                    return Json(new { success = true, responseText = "Hasło zostało zmienione" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Podane hasło jest niepoprawne." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, responseText = "Podane dane są niepoprawne." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public void ChangeUserEmail(string newEmail)
        {
            try
            {
                var userId = (int)Session["UserID"];

                var user = db.Users.FirstOrDefault(s => s.UserID == userId);

                if (user != null)
                {
                    user.Email = newEmail;
                    db.Users.AddOrUpdate(user);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
            }
        }

    }
}