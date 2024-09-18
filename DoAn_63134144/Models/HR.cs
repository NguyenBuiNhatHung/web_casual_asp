//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoAn_63134144.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HR()
        {
            this.JOBS = new HashSet<JOB>();
            this.BOOKS = new HashSet<BOOK>();
        }
    
        public string Id_hr { get; set; }
        public string Ho { get; set; }
        public string TenLot { get; set; }
        public string Ten { get; set; }
        public string Ten_KS { get; set; }
        public Nullable<int> Star { get; set; }
        public string Sdt { get; set; }
        public string Email { get; set; }
        public string Diachi { get; set; }
        public Nullable<decimal> Thanhtoan { get; set; }
        public Nullable<int> Kind { get; set; }
        public string images { get; set; }
        public string Short_name { get; set; }
        public byte[] PasswordHash { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JOB> JOBS { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BOOK> BOOKS { get; set; }
    }
}
