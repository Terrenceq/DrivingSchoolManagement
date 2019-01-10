using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrivingSchoolManagement.ViewModels
{
    public class AccountDetailsViewModel
    {
        public int UserID { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PESEL { get; set; }
        public string Email { get; set; }
        public string About { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string AssignedDriverName { get; set; }
        public string CategoryName { get; set; }
        public int? AssignedStudentsCount { get; set; }
    }
}