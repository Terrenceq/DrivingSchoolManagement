using DrivingSchoolDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingSchoolManagement.ViewModels
{
    public class MyLessonsViewModel
    {
        public Lesson _lesson;
        public int LessonID { get; set; }
        public string Driver { get; set; }
        public string Student { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LessonDate => GetLessonDate(_lesson);
        public bool CancelButton { get; set; }

        public MyLessonsViewModel(Lesson lesson)
        {
            _lesson = lesson;
            LessonID = lesson.LessonID;
            Driver = $"{lesson.DriverInfo.User.FirstName} {lesson.DriverInfo.User.LastName}";
            Student = $"{lesson.StudentInfo.User.FirstName} {lesson.StudentInfo.User.LastName}";
            DateCreated = lesson.DateCreated;
            CancelButton = LessonDate >= DateTime.Now.AddHours(24) ? true : false;
        }

        public DateTime GetLessonDate(Lesson lesson)
        {
            var date = lesson.LessonDate;
            var time = DateTime.Parse(lesson.LessonTime.Time);

            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }
    }
}