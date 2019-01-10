using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

namespace DrivingSchoolManagement.ViewModels
{
    public class HomeCarouselViewModel
    {
        public List<User> OurDrivers { get; set; }
        public List<User> OurManagers { get; set; }
        public List<Vehicle> OurVehicles { get; set; }
        public List<Category> OurCategories { get; set; }
    }
}