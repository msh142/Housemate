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
    using System.ComponentModel.DataAnnotations;

    public partial class Checkout
    {
        public int checkout_id { get; set; }
        [Display(Name = "Cart ID")]
        public Nullable<int> cart_id { get; set; }
        [Display(Name = "Customer ID")]
        public Nullable<int> customer_id { get; set; }
        [Display(Name = "Checkout Date")]
        public Nullable<System.DateTime> checkout_date { get; set; }
    
        public virtual Cart Cart { get; set; }
        public virtual CustomerInfo CustomerInfo { get; set; }
    }
}
