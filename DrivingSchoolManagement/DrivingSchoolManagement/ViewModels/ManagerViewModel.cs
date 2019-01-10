using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

namespace DrivingSchoolManagement.ViewModels
{
    public class ManagerViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string PESEL { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public ManagerViewModel() { }

        public ManagerViewModel(User driver)
        {
            UserID = driver.UserID;
            FirstName = driver.FirstName;
            LastName = driver.LastName;
            DateOfBirth = driver.DateOfBirth;
            Email = driver.Email;
            About = driver.About;
            PESEL = driver.PESEL;
            StreetName = driver.Address.StreetName ?? "-";
            Number = driver.Address.Number ?? "-";
            City = driver.Address.City;
            PostalCode = driver.Address.PostalCode;
            Country = driver.Address.Country;
        }
    }
}