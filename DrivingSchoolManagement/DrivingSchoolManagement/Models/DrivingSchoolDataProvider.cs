using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace DrivingSchoolManagement.Models
{
    public static class DrivingSchoolDataProvider
    {
        const string DrivingSchoolDataContextKey = "DrivingSchoolDataContext";
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";

        public static DrivingSchoolManagementEntities DB
        {
            get
            {
                if (HttpContext.Current.Items[DrivingSchoolDataContextKey] == null)
                    HttpContext.Current.Items[DrivingSchoolDataContextKey] = new DrivingSchoolManagementEntities();
                return (DrivingSchoolManagementEntities)HttpContext.Current.Items[DrivingSchoolDataContextKey];
            }
        }

        public static string GetUserNameById(int id)
        {
            var user = DB.Users.FirstOrDefault(x => x.UserID == id);
            return user.FirstName + " " + user.LastName;
        }

        public static bool IsUserStudent(int userId)
        {
            var userType = DB.Users.FirstOrDefault(x => x.UserID == userId && x.PermissionLevelID == 100);

            if (userType != null)
                return true;
            return false;
        }

        public static bool IsUserDriver(int userId)
        {
            var userType = DB.Users.FirstOrDefault(x => x.UserID == userId && x.PermissionLevelID == 200);

            if (userType != null)
                return true;
            return false;
        }

        public static bool IsUserManager(int userId)
        {
            var userType = DB.Users.FirstOrDefault(x => x.UserID == userId && x.PermissionLevelID == 300);

            if (userType != null)
                return true;
            return false;
        }

        public static string GetDriverCategoryById(int userId)
        {
            return DB.DriverInfoes.FirstOrDefault(x => x.DriverID == userId).Category.CategoryName;
        }

        public static IEnumerable GetCategories()
        {
            return DB.Categories.ToList();
        }

        public static IEnumerable GetDrivers()
        {
            var drivers = DB.DriverInfoes.Select(s => s.User).ToList();
            var model = new List<Driver>();
            foreach (var driver in drivers)
            {
                model.Add(new Driver() { FullName = driver.FirstName + " " + driver.LastName, AssignedDriverID = driver.UserID });
            }

            return model;
        }

        public static IEnumerable GetVehiclesTypes()
        {
            return DB.VehicleTypes.ToList();
        }

        public static List<Driver> GetDriversForUserCategory(int? categoryId)
        {
            var drivers = new List<User>();
            var model = new List<Driver>();

            if (categoryId == null)
            {
                drivers = DB.DriverInfoes.Select(s => s.User).ToList();
                foreach (var driver in drivers)
                {
                    model.Add(new Driver() { FullName = driver.FirstName + " " + driver.LastName, AssignedDriverID = driver.UserID, Category = driver.DriverInfo.Category.CategoryName});
                }

                return model;
            }

            drivers = DB.DriverInfoes.Where(w => w.CategoryID == categoryId).Select(s => s.User).ToList();
            model = new List<Driver>();
            foreach (var driver in drivers)
            {
                model.Add(new Driver() { FullName = driver.FirstName + " " + driver.LastName, AssignedDriverID = driver.UserID, Category = driver.DriverInfo.Category.CategoryName });
            }

            return model;
        }

        public static Category GetCategoryById(int id)
        {
            return DB.Categories.FirstOrDefault(x => x.CategoryID == id);
        }

        public static int GetStudentsAssignedForDriver(int userId)
        {
            return DB.AssignedStudents.Count(s => s.DriverID == userId);
        }

        public static int GetVehiclesForCategory(int categoryId)
        {
            return DB.Vehicles.Count(s => s.CategoryID == categoryId);
        }

        public static string GetLessonsForUser(int userId)
        {
            var studentLessons = DB.Lessons.Where(w => w.StudentID == userId);
            int allLessons = studentLessons.Count();
            int incomingLessons = 0;
            foreach (var lesson in studentLessons)
            {
                var lessonDateTime = GetLessonDateTime(lesson);

                if (lessonDateTime > DateTime.Now)
                    incomingLessons++;  
            }

            return $"{incomingLessons} ({allLessons})";
        }

        public static int GetDriversForCategory(int categoryId)
        {
            return DB.DriverInfoes.Count(s => s.CategoryID == categoryId);
        }

        public static int GetStudentsForCategory(int categoryId)
        {
            return DB.StudentInfoes.Count(s => s.CategoryID == categoryId);
        }

        public static List<LessonTime> GetAvailableLessonTimesForSpecifiedDay(string lessonDate, int userId)
        {
            if (lessonDate == null)
                return DB.LessonTimes.ToList();

            var date = DateTime.Parse(lessonDate);
            var driverId = DB.StudentInfoes.FirstOrDefault(x => x.StudentID == userId).AssignedDriverID;

            var reservedTimes = DB.Lessons.Where(w => w.LessonDate == date && w.DriverID == driverId).Select(s => s.LessonTime);

            return DB.LessonTimes.Except(reservedTimes).ToList();       
        }

        public static bool StudentHasIncomingLesson(int studentId)
        {
            var lessons = DB.Lessons.Where(x => x.StudentID == studentId);

            foreach (var lesson in lessons)
            {
                var lessonDateTime = GetLessonDateTime(lesson);

                if (lessonDateTime < DateTime.Now) continue;
                if (lessonDateTime < DateTime.Now.AddHours(12))
                    return true;
            }

            return false;
        }

        public static string EncryptPassword(string password)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        var textBytes = UTF8Encoding.UTF8.GetBytes(password);
                        var bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }

        public static DateTime GetLessonDateTime(Lesson lesson)
        {
            var date = lesson.LessonDate;
            var time = DateTime.Parse(lesson.LessonTime.Time);
            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, time.Second);
        }

        public static string DecryptPassword(string hashPassword)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        var cipherBytes = Convert.FromBase64String(hashPassword);
                        var bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }

        public static string RemoveSpecialCharacters(string accentedStr)
        {
            byte[] tempBytes;
            tempBytes = System.Text.Encoding.GetEncoding("ISO-8859-8").GetBytes(accentedStr);
            string asciiStr = System.Text.Encoding.UTF8.GetString(tempBytes);

            return asciiStr;
        }
    }
}