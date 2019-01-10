using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;
using System.Web.Mvc;
using DrivingSchoolManagement.ViewModels;
using DrivingSchoolManagement.Services;
using System.Data.Entity.Migrations;

namespace DrivingSchoolManagement.Controllers
{
    public class ManagersController : Controller
    {
        DrivingSchoolManagementEntities db = new DrivingSchoolManagementEntities();

        [DSMAuthorize("User")]
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult ManagersGridViewPartial()
        {
            var model = GetModelData();

            return PartialView("_managersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ManagersGridViewPartialAddNew(ManagerViewModel manager)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var address = db.Addresses.FirstOrDefault(x => x.StreetName == manager.StreetName && x.Number == manager.Number && x.City == manager.City && x.PostalCode == manager.PostalCode &&
                    x.Country == manager.Country);
                    if (address == null)
                    {
                        address = new Address() { StreetName = manager.StreetName, Number = manager.Number, City = manager.City, PostalCode = manager.PostalCode, Country = manager.Country };
                        db.Addresses.Add(address);
                    }
                    var newManager = new User()
                    {
                        FirstName = manager.FirstName,
                        LastName = manager.LastName,
                        DateOfBirth = manager.DateOfBirth,
                        PESEL = manager.PESEL,
                        Email = manager.Email,
                        PermissionLevelID = 300,
                        DateCreated = DateTime.Now,
                        Address = address
                    };
                    if (manager.About != null)
                        newManager.About = manager.About;

                    db.Users.Add(newManager);
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

            return PartialView("_managersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ManagersGridViewPartialUpdate(ManagerViewModel updatedEntry)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var managerToUpdate = db.Users.FirstOrDefault(x => x.UserID == updatedEntry.UserID);

                    if (managerToUpdate != null)
                    {
                        managerToUpdate.FirstName = updatedEntry.FirstName;
                        managerToUpdate.LastName = updatedEntry.LastName;
                        managerToUpdate.DateOfBirth = updatedEntry.DateOfBirth;
                        managerToUpdate.PESEL = updatedEntry.PESEL;
                        managerToUpdate.Email = updatedEntry.Email;
                        managerToUpdate.About = updatedEntry.About;

                        managerToUpdate.Address.City = updatedEntry.City;
                        managerToUpdate.Address.StreetName = updatedEntry.StreetName;
                        managerToUpdate.Address.Number = updatedEntry.Number;
                        managerToUpdate.Address.PostalCode = updatedEntry.PostalCode;
                        managerToUpdate.Address.Country = updatedEntry.Country;

                        db.Users.AddOrUpdate(managerToUpdate);
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

            return PartialView("_managersGridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult ManagersGridViewPartialDelete(ManagerViewModel managerToDelete)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var manager = db.Users.FirstOrDefault(s => s.UserID == managerToDelete.UserID);

                    if (manager != null)
                    {
                        db.Users.Remove(manager);
                    }
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = "Coś poszło nie tak, nie udało się usunąć użytkownika.";
                }
            }

            var model = GetModelData();

            return PartialView("_managersGridViewPartial", model);
        }

        public List<ManagerViewModel> GetModelData()
        {
            var model = new List<ManagerViewModel>();
            var managers = db.Users.Where(x => x.PermissionLevelID == 300).ToList();

            foreach (var manager in managers)
            {
                model.Add(new ManagerViewModel(manager));
            }

            return model;
        }
    }
}