using AirlineBooking.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AirlineBooking.Models
{
    public class ManagementViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Flight Name")]
        public string FlightName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "From")]
        public int OriginCity { set; get; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "To")]
        public int DestinationCity { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Departure Date")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Total Seats")]
        public int TotalSeats { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Available Seats")]
        public int AvailableSeats { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Mileage { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Price Per Ticket")]
        public decimal PricePerTicket { get; set; }

        public List<Value> AvailableFlightCitiesSelectList { get; set; } = new List<Value>();

        public ManagementViewModel()
        {
            SetAvailableFlightCities();
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
            catch (Exception ex) { throw ex; }
        }
    }
}