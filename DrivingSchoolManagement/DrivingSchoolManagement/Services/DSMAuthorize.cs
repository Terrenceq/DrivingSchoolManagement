using DrivingSchoolDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolManagement.Models;
using System.Web.Mvc;

namespace DrivingSchoolManagement.Services
{
    public class DSMAuthorize : AuthorizeAttribute
    {
        private readonly string _resource;

        public DSMAuthorize(params string[] paramsStrings)
        {
            _resource = paramsStrings[0];
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;

            if (HttpContext.Current.Session["UserID"] == null)
                return false;

            using (DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities())
            {
                var userId = (int)HttpContext.Current.Session["UserID"];
                var user = db.Users.FirstOrDefault(x => x.UserID == userId);

                if (user != null)
                {
                    if (_resource == "User")
                        authorize = true;

                    if (_resource == "Driver")
                        authorize = DrivingSchoolDataProvider.IsUserDriver(user.UserID) ? true : false;

                    if (_resource == "Manager")
                        authorize = DrivingSchoolDataProvider.IsUserManager(user.UserID) ? true : false; 
                }              
            }
            return authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary
            {
                {"action", "Index" },
                {"controller", "Login" }
            });
        }
    }
}