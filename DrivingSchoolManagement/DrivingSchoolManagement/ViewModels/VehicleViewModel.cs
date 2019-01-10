using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

namespace DrivingSchoolManagement.ViewModels
{
    public class VehicleViewModel
    {
        public int VehicleID { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime ProductionDate { get; set; }
        public int VehicleTypeID { get; set; }
        public string Mileage { get; set; }
        public string About { get; set; }
        public int CategoryID { get; set; }

        public VehicleViewModel() { }

        public VehicleViewModel(Vehicle vehicle)
        {
            VehicleID = vehicle.VehicleID;
            Brand = vehicle.Brand;
            Model = vehicle.Model;
            ProductionDate = vehicle.ProductionDate;
            VehicleTypeID = vehicle.VehicleTypeID;
            Mileage = vehicle.Mileage;
            About = vehicle.About;
            CategoryID = vehicle.CategoryID;
        }
    }
}