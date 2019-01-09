using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

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
    }
}