using DrivingSchoolDb;
using DrivingSchoolManagement.Models;
using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DrivingSchoolManagement.Controllers
{
    public class LessonsController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            Session["LessonDate"] = null;
            return View();
        }

        public ActionResult SaveLessonDateInSession(DateTime lessonDate)
        {
            Session["LessonDate"] = lessonDate.ToShortDateString();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LessonCreateFormLayout()
        {
            var studentId = (int)Session["UserID"];
            var model = new LessonCreateViewModel();
            var driver = db.StudentInfoes.FirstOrDefault(x => x.StudentID == studentId).DriverInfo.User;

            var student = db.Users.FirstOrDefault(x => x.UserID == studentId);

            model.Student = $"{student.FirstName} {student.LastName}";
            model.Driver = $"{driver.FirstName} {driver.LastName}";

            return PartialView("_lessonCreateFormLayout", model);
        }

        public ActionResult SaveLessonInDB(LessonCreateViewModel item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var driver = db.Users.FirstOrDefault(x => x.FirstName + " " + x.LastName == item.Driver);
                    var student = db.Users.FirstOrDefault(x => x.FirstName + " " + x.LastName == item.Student);
                    var lessonDate = DateTime.Parse((string)Session["LessonDate"]);
                    var newLesson = new Lesson()
                    {
                        DateCreated = DateTime.Now,
                        DriverID = driver.UserID,
                        StudentID = student.UserID,
                        LessonDate = lessonDate,
                        LessonTimeID = item.LessonTimeID
                    };

                    db.Lessons.Add(newLesson);
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
            return RedirectToAction("Index");
        }

        [DSMAuthorize("User")]
        public ActionResult MyLessons()
        {
            return PartialView();
        }

        public ActionResult MyLessonsGridViewPartial()
        {
            var model = new List<MyLessonsViewModel>();
            var userId = (int)Session["UserID"];
            List<Lesson> myLessons;

            if (DrivingSchoolDataProvider.IsUserStudent(userId))
                myLessons = db.Lessons.Where(x => x.StudentID == userId).ToList();
            else
                myLessons = db.Lessons.Where(x => x.DriverID == userId).ToList();

            foreach (var lesson in myLessons)
            {
                model.Add(new MyLessonsViewModel(lesson));
            }

            return PartialView("_myLessonsGridViewPartial", model);
        }

        public void CancelLesson(int lessonId)
        {
            var deleteLesson = db.Lessons.FirstOrDefault(x => x.LessonID == lessonId);

            db.Lessons.Remove(deleteLesson);
            db.SaveChanges();
        }
    }
}