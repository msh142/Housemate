//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Housemate.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceRequested
    {
        public int req_id { get; set; }
        public Nullable<int> service_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<System.DateTime> req_date { get; set; }
        public Nullable<System.TimeSpan> req_time { get; set; }
        public string req_address { get; set; }
        public string req_district { get; set; }
        public string req_status { get; set; }
    
        public virtual CustomerInfo CustomerInfo { get; set; }
        public virtual Service Service { get; set; }
    }
}
