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
    
    public partial class FLIGHT_AVAILABLITY
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int TotalAvaliableSeats { get; set; }
        public int CurrentSeatsTaken { get; set; }
    
        public virtual FLIGHT_SCHEDULE FLIGHT_SCHEDULE { get; set; }
    }
}
