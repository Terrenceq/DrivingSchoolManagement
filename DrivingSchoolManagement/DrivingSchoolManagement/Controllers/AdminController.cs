using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Web.Mvc;
using DrivingSchoolManagement.Models;
using System.Data.Entity.Migrations;

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

        [DSMAuthorize("Manager")]
        public ActionResult Students()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult StudentsGridViewPartial()
        {
            var model = new List<StudentViewModel>();
            var students = db.Users.Where(x => x.PermissionLevelID == 100).ToList();

            foreach (var student in students)
            {
                model.Add(new StudentViewModel(student));
            }

            return PartialView("_studentsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StudentsGridViewPartialAddNew(StudentViewModel student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var address = db.Addresses.FirstOrDefault(x => x.StreetName == student.StreetName && x.Number == student.Number && x.City == student.City && x.PostalCode == student.PostalCode &&
                    x.Country == student.Country);
                    if (address == null)
                    {
                        address = new Address() { StreetName = student.StreetName, Number = student.Number, City = student.City, PostalCode = student.PostalCode, Country = student.Country };
                        db.Addresses.Add(address);
                    }
                    var newStudent = new User()
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        DateOfBirth = student.DateOfBirth,
                        PESEL = student.PESEL,
                        Email = student.Email,
                        PermissionLevelID = 100,
                        DateCreated = DateTime.Now,
                        Address = address
                    };
                    if (student.About != null)
                        newStudent.About = student.About;

                    var newStudentInfo = new StudentInfo() { User = newStudent, AssignedDriverID = student.AssignedDriverID, CategoryID = student.CategoryID };

                    var newAssignedStudent = new AssignedStudent() { DriverID = student.AssignedDriverID, StudentID = newStudent.UserID };

                    var newStudentUserCredentials = new UserCredential
                    {
                        User = newStudent,
                        Login = DrivingSchoolDataProvider.RemoveSpecialCharacters(newStudent.FirstName + "." + newStudent.LastName).ToLower(),
                        Password = DrivingSchoolDataProvider.EncryptPassword(newStudent.PESEL)
                    };

                    db.Users.Add(newStudent);
                    db.StudentInfoes.Add(newStudentInfo);
                    db.AssignedStudents.Add(newAssignedStudent);
                    db.UserCredentials.Add(newStudentUserCredentials);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";


            var model = new List<StudentViewModel>();
            var students = db.Users.Where(x => x.PermissionLevelID == 100).ToList();

            foreach (var singleStudent in students)
            {
                model.Add(new StudentViewModel(singleStudent));
            }

            return PartialView("_studentsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StudentsGridViewPartialUpdate(StudentViewModel updatedEntry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var studentToUpdate = db.Users.FirstOrDefault(x => x.UserID == updatedEntry.UserID);

                    if (studentToUpdate != null)
                    {
                        studentToUpdate.FirstName = updatedEntry.FirstName;
                        studentToUpdate.LastName = updatedEntry.LastName;
                        studentToUpdate.DateOfBirth = updatedEntry.DateOfBirth;
                        studentToUpdate.PESEL = updatedEntry.PESEL;
                        studentToUpdate.Email = updatedEntry.Email;
                        studentToUpdate.About = updatedEntry.About;

                        studentToUpdate.Address.City = updatedEntry.City;
                        studentToUpdate.Address.StreetName = updatedEntry.StreetName;
                        studentToUpdate.Address.Number = updatedEntry.Number;
                        studentToUpdate.Address.PostalCode = updatedEntry.PostalCode;
                        studentToUpdate.Address.Country = updatedEntry.Country;

                        studentToUpdate.StudentInfo.AssignedDriverID = updatedEntry.AssignedDriverID;
                        studentToUpdate.StudentInfo.CategoryID = updatedEntry.CategoryID;

                        var assignedStudent = db.AssignedStudents.FirstOrDefault(s => s.StudentID == studentToUpdate.UserID);
                        assignedStudent.DriverID = updatedEntry.AssignedDriverID;

                        db.Users.AddOrUpdate(studentToUpdate);
                        db.AssignedStudents.AddOrUpdate(assignedStudent);
                        db.SaveChanges();
                    }

                    else
                        ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";

            var model = new List<StudentViewModel>();
            var students = db.Users.Where(x => x.PermissionLevelID == 100).ToList();

            foreach (var singleStudent in students)
            {
                model.Add(new StudentViewModel(singleStudent));
            }

            return PartialView("_studentsGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult StudentsGridViewPartialDelete(DriverViewModel studentToDelete)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var student = db.Users.FirstOrDefault(s => s.UserID == studentToDelete.UserID);

                    if (student != null)
                    {
                        db.Users.Remove(student);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, nie udało się usunąć użytkownika.";
                }
            }

            var model = new List<StudentViewModel>();
            var students = db.Users.Where(x => x.PermissionLevelID == 100).ToList();

            foreach (var singleStudent in students)
            {
                model.Add(new StudentViewModel(singleStudent));
            }

            return PartialView("_studentsGridViewPartial", model);
        }
    }
}