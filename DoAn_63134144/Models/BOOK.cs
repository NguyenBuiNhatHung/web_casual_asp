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
    
    public partial class BOOK
    {
        public string Id_book { get; set; }
        public string Id_hr { get; set; }
        public Nullable<System.DateTime> Ngay { get; set; }
        public Nullable<System.TimeSpan> Tstart { get; set; }
        public Nullable<System.TimeSpan> Tend { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Support { get; set; }
        public string Location { get; set; }
        public string Note { get; set; }
        public Nullable<int> sl { get; set; }
        public Nullable<int> need { get; set; }
    
        public virtual HR HR { get; set; }
    }
}
