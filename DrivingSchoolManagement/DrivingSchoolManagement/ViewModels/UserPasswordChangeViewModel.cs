using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingSchoolManagement.ViewModels
{
    public class UserPasswordChangeViewModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }
    }
}