//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DenisovArt_Kurs
{
    using System;
    using System.Collections.Generic;
    
    public partial class VisitList
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VisitList()
        {
            this.OrderedServices = new HashSet<OrderedService>();
        }
    
        public int VisitListID { get; set; }
        public int ClientID { get; set; }
        public System.DateTime VisitDate { get; set; }
        public decimal PriceTotal { get; set; }
    
        public virtual Client Client { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderedService> OrderedServices { get; set; }
    }
}
