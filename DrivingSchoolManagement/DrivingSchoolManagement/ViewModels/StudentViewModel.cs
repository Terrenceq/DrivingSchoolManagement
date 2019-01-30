using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DrivingSchoolDb;

namespace DrivingSchoolManagement.ViewModels
{
    public class StudentViewModel
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public int CategoryID { get; set; }
        public string About { get; set; }
        public string PESEL { get; set; }
        public string StreetName { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public int AssignedDriverID { get; set; }

        public StudentViewModel() { }

        public StudentViewModel(User student)
        {
            UserID = student.UserID;
            FirstName = student.FirstName;
            LastName = student.LastName;
            DateOfBirth = student.DateOfBirth;
            Email = student.Email;
            About = student.About;
            PESEL = student.PESEL;
            CategoryID = student.StudentInfo.CategoryID;
            StreetName = student.Address.StreetName ?? "-";
            Number = student.Address.Number ?? "-";
            City = student.Address.City;
            PostalCode = student.Address.PostalCode;
            Country = student.Address.Country;
            AssignedDriverID = student.StudentInfo.AssignedDriverID;
        }
    }
}