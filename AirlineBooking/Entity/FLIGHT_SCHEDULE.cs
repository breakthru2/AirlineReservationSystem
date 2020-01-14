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
    
    public partial class FLIGHT_SCHEDULE
    {
        public FLIGHT_SCHEDULE()
        {
            this.FLIGHT_AVAILABLITY = new HashSet<FLIGHT_AVAILABLITY>();
            this.FLIGHT_RESERVATION = new HashSet<FLIGHT_RESERVATION>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Mileage { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> DepartureDate { get; set; }
        public Nullable<decimal> PricePerTicket { get; set; }
    
        public virtual AVAILABLE_FLIGHT_CITIES AVAILABLE_FLIGHT_CITIES { get; set; }
        public virtual AVAILABLE_FLIGHT_CITIES AVAILABLE_FLIGHT_CITIES1 { get; set; }
        public virtual ICollection<FLIGHT_AVAILABLITY> FLIGHT_AVAILABLITY { get; set; }
        public virtual ICollection<FLIGHT_RESERVATION> FLIGHT_RESERVATION { get; set; }
    }
}
