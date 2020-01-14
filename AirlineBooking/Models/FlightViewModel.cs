using AirlineBooking.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AirlineBooking.Models
{
    public class FlightViewModel
    {
        /// <summary>
        /// Holds the Id of a flight as produced by Entity Framework
        /// </summary>
        public int FlightId { get; set; }

        /// <summary>
        /// Holds the name of a  flight
        /// </summary>
        [Display(Name ="Flight Name")]
        public string FlightName { get; set; }

        public int TotalSeats { get; set; }

        public int CurrentlyAvailableSeats { get; set; }

        public decimal PricePerTicket { get; set; }

        /// <summary>
        /// Holds the String representation of an Origin City
        /// </summary>
        [Display(Name = "Origin City")]
        public string OriginCity { get; set; }

        /// <summary>
        /// Holds the String representation of a Destination City
        /// </summary>
        [Display(Name = "Destination City")]
        public string DestinationCity { get; set; }

        /// <summary>
        /// Holds the interger representation of an Origin City
        /// </summary>
        public int OriginCityId { get; set; }

        /// <summary>
        /// Holds the interger representation of Destination city
        /// </summary>
        public int DestinationCityId { get; set; }

        [Display(Name = "Departure Date")]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; }

        [Display(Name = "Number Of Tickets")]
        public int NumberOfTickets { get; set; }

        [Display(Name = "Class")]
        public int FlightClass { get; set; }
        
        [Display(Name = "Type of trip")]
        public int TripType { get; set; }

        public bool IsOneWayTrip { get; set; }

        public bool IsRoundTrip { get; set; }

        public FlightSummary FlightSummary { get; set; } = new FlightSummary();

        public FlightSummary ReturnFlightSummary { get; set; } = new FlightSummary();

        [Display(Name = "Confirmation Number")]
        public string BlockingNumber { get; set; }

        /// <summary>
        /// Holds a list of all Trip Types. E.G One-way Trip, Round Trip
        /// </summary>
        public List<Value> TripTypesSelectList { get; set; } = new List<Value>();

        /// <summary>
        /// Holds a list for all avlaible Flight classes E.G business class
        /// </summary>
        public List<Value> FlightClassesSelectList { get; set; } = new List<Value>();

        public List<Value> AvailableFlightCitiesSelectList { get; set; } = new List<Value>();

        /// <summary>
        /// Holds a list of a reservations made for a given Flight
        /// </summary>
        public List<FlightReservation> FlightReservations { get; set; } = new List<FlightReservation>();

        public List<FlightList> FlightList { get; set; } = new List<FlightList>();

        public List<FlightList> ConnectingFlightList { get; set; } = new List<FlightList>();

        public FlightViewModel()
        {
            SetAvailableFlightCities();
            SetFlightClasses();
            SetTripTypes();
        }
        
        public void SetFlightClasses()
        {
            try
            {
                ARS_AIRLINESEntities1 db = new ARS_AIRLINESEntities1();
                var flightClasses = db.FLIGHT_CLASS.Where(f => f.Active).ToList();
                
                if (flightClasses?.Count() > 0)
                {
                    FlightClassesSelectList.Add(new Value() { Id = 0, Text = "--Select Flight Class--" });
                    foreach (var item in flightClasses)
                    {
                        FlightClassesSelectList.Add(new Value() { Id = item.Id, Text = item.Name });
                    }
                }
            }
            catch(Exception ex) { throw ex; }
        }

        public void SetTripTypes()
        {
            try
            {
                ARS_AIRLINESEntities1 db = new ARS_AIRLINESEntities1();
                var tripTypes = db.TRIP_TYPE.Where(t => t.Active).ToList();

                if (tripTypes?.Count() > 0)
                {
                    TripTypesSelectList.Add(new Value() { Id = 0, Text = "--Select Trip Type--" });
                    foreach (var item in tripTypes)
                    {
                        TripTypesSelectList.Add(new Value() { Id = item.Id, Text = item.Name });
                    }
                }
            }
            catch(Exception ex) { throw ex; }
        }

        public void SetAvailableFlightCities()
        {
            try
            {
                ARS_AIRLINESEntities1 db = new ARS_AIRLINESEntities1();
                var availableFlightCities = db.AVAILABLE_FLIGHT_CITIES.Where(f => f.Active).ToList();

                if (availableFlightCities?.Count() > 0)
                {
                    AvailableFlightCitiesSelectList.Add(new Value() { Id = 0, Text = "--Select City--" });
                    foreach (var item in availableFlightCities)
                    {
                        AvailableFlightCitiesSelectList.Add(new Value() { Id = item.Id, Text = item.Name });
                    }
                }
            }
            catch(Exception ex) { throw ex; }
        }
    }

    public class FlightList
    {
        public int FlightId { get; set; }

        public string FlightName { get; set; }

        public string OriginCity { get; set; }

        public string DestinationCity { get; set; }

        public string StopOverCity { get; set; }

        public int Mileage { get; set; }

        public string DepartureDate { get; set; }

        public int TotalSeats { get; set; }

        public int AvailableSeats { get; set; }
    }

    public class FlightReservation
    {
        public int FlightId { get; set; }

        public int UserId { get; set; }

        public string Status { get; set; }

        public string BlockingNumber { get; set; }

        public string CancellationNumber { get; set; }
    }

    public class FlightSummary
    {
        public int FlightId { get; set; }

        public string FlightName { get; set; }

        public int NumberOfTickets { get; set; }

        public DateTime DepartureDate { get; set; }

        public decimal PricePerTicket { get; set; }

        public string From { get; set; }

        public string To { get; set; }
    }
}