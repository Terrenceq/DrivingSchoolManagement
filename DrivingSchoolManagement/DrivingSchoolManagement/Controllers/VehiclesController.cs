using DrivingSchoolManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DrivingSchoolDb;
using DrivingSchoolManagement.ViewModels;
using System.Data.Entity.Migrations;

namespace DrivingSchoolManagement.Controllers
{
    public class VehiclesController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult VehiclesGridViewPartial()
        {
            var model = GetModelData();
            return PartialView("_vehiclesGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult VehiclesGridViewPartialAddNew(VehicleViewModel vehicle)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var newVehicle = new Vehicle()
                    {
                        Brand = vehicle.Brand,
                        Model = vehicle.Model,
                        ProductionDate = vehicle.ProductionDate,
                        VehicleTypeID = vehicle.VehicleTypeID,
                        Mileage = vehicle.Mileage,
                        About = vehicle.About,
                        CategoryID = vehicle.CategoryID
                    };

                    db.Vehicles.Add(newVehicle);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";


            var model = GetModelData();
            return PartialView("_vehiclesGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult VehiclesGridViewPartialUpdate(VehicleViewModel updatedEntry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vehicleToUpdate = db.Vehicles.FirstOrDefault(x => x.VehicleID == updatedEntry.VehicleID);

                    if (vehicleToUpdate != null)
                    {
                        vehicleToUpdate.Brand = updatedEntry.Brand;
                        vehicleToUpdate.Model = updatedEntry.Model;
                        vehicleToUpdate.ProductionDate = updatedEntry.ProductionDate;
                        vehicleToUpdate.VehicleTypeID = updatedEntry.VehicleTypeID;
                        vehicleToUpdate.Mileage = updatedEntry.Mileage;
                        vehicleToUpdate.About = updatedEntry.About;
                        vehicleToUpdate.CategoryID = updatedEntry.CategoryID;

                        db.Vehicles.AddOrUpdate(vehicleToUpdate);
                        db.SaveChanges();
                    }

                    else
                        ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";

                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";

            var model = GetModelData();

            return PartialView("_vehiclesGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult VehiclesGridViewPartialDelete(VehicleViewModel vehicleToDelete)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var vehicle = db.Vehicles.FirstOrDefault(s => s.VehicleID == vehicleToDelete.VehicleID);

                    if (vehicle != null)
                    {
                        db.Vehicles.Remove(vehicle);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, nie udało się usunąć pojazdu.";
                }
            }

            var model = GetModelData();

            return PartialView("_vehiclesGridViewPartial", model);
        }

        public List<VehicleViewModel> GetModelData()
        {
            var model = new List<VehicleViewModel>();
            var vehicles = db.Vehicles.ToList();

            foreach (var vehicle in vehicles)
            {
                model.Add(new VehicleViewModel(vehicle));
            }

            return model;
        }
    }
}