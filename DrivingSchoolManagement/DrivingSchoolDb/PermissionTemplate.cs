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
    
    public partial class PermissionTemplate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PermissionTemplate()
        {
            this.Permissions = new HashSet<Permission>();
        }
    
        public int PermissionTemplateID { get; set; }
        public string PermissionTemplateName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Permission> Permissions { get; set; }
    }
}
