using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System.Web.Mvc;

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
    }
}