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
    
    public partial class VisitReason
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VisitReason()
        {
            this.VisitInfo = new HashSet<VisitInfo>();
        }
    
        public short VSR_Id { get; set; }
        public string VSR_Name { get; set; }
        public Nullable<short> VSR_Order { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VisitInfo> VisitInfo { get; set; }
    }
}
