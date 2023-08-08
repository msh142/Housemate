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
    
    public partial class BuyHistory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BuyHistory()
        {
            this.BuyRecords = new HashSet<BuyRecord>();
        }
    
        public int history_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<System.DateTime> purchase_date { get; set; }
    
        public virtual CustomerInfo CustomerInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyRecord> BuyRecords { get; set; }
    }
}
