//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DrivingSchoolDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.UserCredentials = new HashSet<UserCredential>();
        }
    
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string PESEL { get; set; }
        public string Email { get; set; }
        public int PermissionLevelID { get; set; }
        public string About { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int AddressID { get; set; }
    
        public virtual Address Address { get; set; }
        public virtual DriverInfo DriverInfo { get; set; }
        public virtual PermissionLevel PermissionLevel { get; set; }
        public virtual StudentInfo StudentInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserCredential> UserCredentials { get; set; }
    }
}
