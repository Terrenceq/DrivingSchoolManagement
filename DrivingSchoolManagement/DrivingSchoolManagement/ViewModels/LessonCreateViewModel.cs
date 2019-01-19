using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingSchoolManagement.ViewModels
{
    public class LessonCreateViewModel
    {
        public int LessonTimeID { get; set; }
        public string Student { get; set; }
        public string Driver { get; set; }
    }
}