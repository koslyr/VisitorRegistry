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
    
    public partial class UserUserGroup
    {
        public short UUG_Id { get; set; }
        public short UUG_USE_Id { get; set; }
        public short UUG_USG_Id { get; set; }
    
        public virtual UserGroup UserGroup { get; set; }
        public virtual User User { get; set; }
    }
}
