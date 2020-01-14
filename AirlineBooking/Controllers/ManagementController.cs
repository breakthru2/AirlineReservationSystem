using AirlineBooking.Controllers;
using AirlineBooking.Models;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Transactions;

namespace AirlineBooking.Views.Home
{
    public class ManagementController : BaseController
    {
        private ManagementViewModel viewModel;

        public ManagementController()
        {
            viewModel = new ManagementViewModel();
        }

        // GET: Management
        [HttpGet]
        public ActionResult AddFlight()
        {
            try
            {
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        [HttpPost]
        public ActionResult AddFlight(ManagementViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentFlight = db.FLIGHT_SCHEDULE.Where(f => f.Name == viewModel.FlightName && f.DepartureDate == viewModel.DepartureDate).FirstOrDefault();

                    if (currentFlight == null)
                    {
                        //using (TransactionScope transaction = new TransactionScope())
                        //{

                        //}
                        var flight = new Entity.FLIGHT_SCHEDULE()
                        {
                            Mileage = viewModel.Mileage,
                            DateCreated = DateTime.Now,
                            Name = viewModel.FlightName,
                            To = viewModel.DestinationCity,
                            From = viewModel.OriginCity,
                            DepartureDate = viewModel.DepartureDate,
                            PricePerTicket = viewModel.PricePerTicket
                        };

                        var returnedFlight = db.FLIGHT_SCHEDULE.Add(flight);

                        var flightAvailablility = new Entity.FLIGHT_AVAILABLITY()
                        {
                            FlightId = returnedFlight.Id,
                            CurrentSeatsTaken = viewModel.TotalSeats,
                            TotalAvaliableSeats = viewModel.TotalSeats
                        };

                        db.FLIGHT_AVAILABLITY.Add(flightAvailablility);

                        db.SaveChanges();

                        SetMessage(new Message() { MessageText = "Added Flight", MessageCategory = (int)MessageCategory.SUCCESS });

                        return RedirectToAction("AddFlight");
                    }
                    else
                    {
                        SetMessage(new Message() { MessageText = "Similar Flight already exists", MessageCategory = (int)MessageCategory.ERROR });
                    }
                }
                return View(viewModel);
            }
            catch (Exception ex) { throw ex; }
        }


    }
}