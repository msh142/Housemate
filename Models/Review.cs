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
    
    public partial class Review
    {
        public int review_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<int> rating { get; set; }
        public string comment { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
    
        public virtual CustomerInfo CustomerInfo { get; set; }
        public virtual Product Product { get; set; }
    }
}
