using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using DrivingSchoolManagement.Models;

namespace DrivingSchoolManagement.ViewModels
{
    public class AdminPanelViewModel
    {
        [Display(Name = "Kursanci")]
        public int StudentsCount { get; set; }
        [Display(Name = "Instruktorzy")]
        public int DriversCount { get; set; }
        [Display(Name = "Menadżerowie")]
        public int ManagersCount { get; set; }
        [Display(Name = "Pojazdy")]
        public int VehiclesCount { get; set; }
        [Display(Name = "Kategorie")]
        public int CategoriesCount { get; set; }
        [Display(Name = "Wszystkie spotkania")]
        public int LessonsCount { get; set; }
        [Display(Name = "Przyszłe spotkania")]
        public int IncomingLessonsCount => GetIncomingLessonsCount();
        [Display(Name = "Przeszłe spotkania")]
        public int PastLessonsCount { get; set; }

        public AdminPanelViewModel(DrivingSchoolDb.DrivingSchoolManagementEntities db)
        {
            ManagersCount = db.Users.Where(w => w.PermissionLevelID == 300).Count();
            DriversCount = db.Users.Where(w => w.PermissionLevelID == 200).Count();
            StudentsCount = db.Users.Where(w => w.PermissionLevelID == 100).Count();
            VehiclesCount = db.Vehicles.Count();
            CategoriesCount = db.Categories.Count();
            LessonsCount = db.Lessons.Count();
            PastLessonsCount = LessonsCount - IncomingLessonsCount;
        }

        private int GetIncomingLessonsCount()
        {
            int count = 0;
            using (var db = new DrivingSchoolManagementEntities())
            {
                var lessons = db.Lessons.ToList();
                foreach (var lesson in lessons)
                {
                    if (DrivingSchoolDataProvider.GetLessonDateTime(lesson) > DateTime.Now)
                        count++;
                } 
            }
            return count;
        }
    }
}