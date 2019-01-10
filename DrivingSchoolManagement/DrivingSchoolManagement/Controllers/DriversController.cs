using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Web.Mvc;
using DrivingSchoolManagement.Services;
using DrivingSchoolManagement.ViewModels;
using System.Data.Entity.Migrations;

namespace DrivingSchoolManagement.Controllers
{
    public class DriversController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult DriversGridViewPartial()
        {
            var model = new List<DriverViewModel>();
            var drivers = db.Users.Where(x => x.PermissionLevelID == 200).ToList();

            foreach (var driver in drivers)
            {
                model.Add(new DriverViewModel(driver));
            }

            return PartialView("_driversGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DriversGridViewPartialAddNew(DriverViewModel driver)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var address = db.Addresses.FirstOrDefault(x => x.StreetName == driver.StreetName && x.Number == driver.Number && x.City == driver.City && x.PostalCode == driver.PostalCode &&
                    x.Country == driver.Country);
                    if (address == null)
                    {
                        address = new Address(){StreetName = driver.StreetName, Number = driver.Number, City = driver.City, PostalCode = driver.PostalCode, Country = driver.Country };
                        db.Addresses.Add(address);
                    }
                    var newDriver = new User()
                    {
                        FirstName = driver.FirstName,
                        LastName = driver.LastName,
                        DateOfBirth = driver.DateOfBirth,
                        PESEL = driver.PESEL,
                        Email = driver.Email,
                        PermissionLevelID = 200,
                        DateCreated = DateTime.Now,
                        Address = address
                    };
                    if (driver.About != null)
                        newDriver.About = driver.About;

                    var newDriverInfo = new DriverInfo() { User = newDriver, LicenceNumber = driver.LicenceNumber, CategoryID = driver.CategoryID };

                    db.Users.Add(newDriver);
                    db.DriverInfoes.Add(newDriverInfo);

                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";


            var model = new List<DriverViewModel>();
            var drivers = db.Users.Where(x => x.PermissionLevelID == 200).ToList();

            foreach (var drive in drivers)
            {
                model.Add(new DriverViewModel(drive));
            }

            return PartialView("_driversGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DriversGridViewPartialUpdate(DriverViewModel updatedEntry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var driverToUpdate = db.Users.FirstOrDefault(x => x.UserID == updatedEntry.UserID);

                    if (driverToUpdate != null)
                    {
                        driverToUpdate.FirstName = updatedEntry.FirstName;
                        driverToUpdate.LastName = updatedEntry.LastName;
                        driverToUpdate.DateOfBirth = updatedEntry.DateOfBirth;
                        driverToUpdate.PESEL = updatedEntry.PESEL;
                        driverToUpdate.Email = updatedEntry.Email;
                        driverToUpdate.About = updatedEntry.About;

                        driverToUpdate.Address.City = updatedEntry.City;
                        driverToUpdate.Address.StreetName = updatedEntry.StreetName;
                        driverToUpdate.Address.Number = updatedEntry.Number;
                        driverToUpdate.Address.PostalCode = updatedEntry.PostalCode;
                        driverToUpdate.Address.Country = updatedEntry.Country;

                        driverToUpdate.DriverInfo.LicenceNumber = updatedEntry.LicenceNumber;
                        driverToUpdate.DriverInfo.CategoryID = updatedEntry.CategoryID;

                        db.Users.AddOrUpdate(driverToUpdate);
                        db.SaveChanges();
                    }

                    else
                        ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
                catch(Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";
                }
            }
            else
                ViewData["EditError"] = "Coś poszło nie tak, popraw dane i spróbuj ponownie.";

            var model = new List<DriverViewModel>();
            var drivers = db.Users.Where(x => x.PermissionLevelID == 200).ToList();

            foreach (var drive in drivers)
            {
                model.Add(new DriverViewModel(drive));
            }

            return PartialView("_driversGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult DriversGridViewPartialDelete(DriverViewModel driverToDelete)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var driver = db.Users.FirstOrDefault(s => s.UserID == driverToDelete.UserID);

                    if (driver != null)
                    {
                        db.Users.Remove(driver);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, nie udało się usunąć użytkownika.";
                }
            }

            var model = new List<DriverViewModel>();
            var drivers = db.Users.Where(x => x.PermissionLevelID == 200).ToList();

            foreach (var drive in drivers)
            {
                model.Add(new DriverViewModel(drive));
            }

            return PartialView("_driversGridViewPartial", model);
        }

    }
}