using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Collections;

namespace DrivingSchoolManagement.Models
{
    public static class DrivingSchoolDataProvider
    {
        const string DrivingSchoolDataContextKey = "DrivingSchoolDataContext";

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

        public static Category GetCategoryById(int id)
        {
            return DB.Categories.FirstOrDefault(x => x.CategoryID == id);
        }

        public static int GetStudentsAssignedForDriver(int userId)
        {
            return DB.AssignedStudents.Count(s => s.DriverID == userId);
        }
    }
}