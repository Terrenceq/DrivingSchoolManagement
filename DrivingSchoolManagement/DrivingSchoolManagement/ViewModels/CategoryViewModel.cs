using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

namespace DrivingSchoolManagement.ViewModels
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string About { get; set; }

        public CategoryViewModel(Category category)
        {
            CategoryID = category.CategoryID;
            CategoryName = category.CategoryName;
            About = category.About;
        }
    }
}