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
    
    public partial class Wishlist
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wishlist()
        {
            this.WishlistRecords = new HashSet<WishlistRecord>();
        }
    
        public int wishlist_id { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> product_id { get; set; }
    
        public virtual CustomerInfo CustomerInfo { get; set; }
        public virtual Product Product { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WishlistRecord> WishlistRecords { get; set; }
    }
}
