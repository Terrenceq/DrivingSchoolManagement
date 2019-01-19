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

        public static IEnumerable GetVehiclesTypes()
        {
            return DB.VehicleTypes.ToList();
        }

        public static Category GetCategoryById(int id)
        {
            return DB.Categories.FirstOrDefault(x => x.CategoryID == id);
        }

        public static int GetStudentsAssignedForDriver(int userId)
        {
            return DB.AssignedStudents.Count(s => s.DriverID == userId);
        }

        public static List<LessonTime> GetAvailableLessonTimesForSpecifiedDay(string lessonDate)
        {
            if (lessonDate == null)
                return DB.LessonTimes.ToList();

            var date = DateTime.Parse(lessonDate);

            var reservedTimes = DB.Lessons.Where(w => w.LessonDate == date).Select(s => s.LessonTime);

            return DB.LessonTimes.Except(reservedTimes).ToList();
           
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