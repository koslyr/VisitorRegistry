//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VisReg.Dal.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Visitor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Visitor()
        {
            this.VisitInfo = new HashSet<VisitInfo>();
        }
    
        public int VST_Id { get; set; }
        public Nullable<short> VST_SEX_Id { get; set; }
        public Nullable<short> VST_VSC_Id { get; set; }
        public Nullable<short> VST_IDC_Id { get; set; }
        public string VST_LastName { get; set; }
        public string VST_FirstName { get; set; }
        public string VST_FatherName { get; set; }
        public string VST_MotherName { get; set; }
        public string VST_IdentificationNumber { get; set; }
        public string VST_Address { get; set; }
        public string VST_PostalCode { get; set; }
        public string VST_MobileNumber { get; set; }
        public string VST_Email { get; set; }
        public string VST_Description { get; set; }
    
        public virtual IdentificationCertificate IdentificationCertificate { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual VisitorCategory VisitorCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitInfo> VisitInfo { get; set; }
    }
}
