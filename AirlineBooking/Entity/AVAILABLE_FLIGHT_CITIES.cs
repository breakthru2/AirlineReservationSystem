//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AirlineBooking.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class AVAILABLE_FLIGHT_CITIES
    {
        public AVAILABLE_FLIGHT_CITIES()
        {
            this.FLIGHT_SCHEDULE = new HashSet<FLIGHT_SCHEDULE>();
            this.FLIGHT_SCHEDULE1 = new HashSet<FLIGHT_SCHEDULE>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    
        public virtual ICollection<FLIGHT_SCHEDULE> FLIGHT_SCHEDULE { get; set; }
        public virtual ICollection<FLIGHT_SCHEDULE> FLIGHT_SCHEDULE1 { get; set; }
    }
}
