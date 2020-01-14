using AirlineBooking.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineBooking.Controllers
{
    public class FlightController : BaseController
    {
        private FlightViewModel viewModel;

        public FlightController()
        {
            viewModel = new FlightViewModel();
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: Flight
        [Authorize]
        [HttpGet]
        public ActionResult BookFlight(int Id)
        {
            try
            {
                viewModel.FlightId = Id;
                var flight = db.FLIGHT_SCHEDULE.Where(f => f.Id == Id).FirstOrDefault();
                if(flight != null)
                {
                    string msg = string.Format("The departure date for this flight is: {0}", flight.DepartureDate);
                    SetMessage(new Message() { MessageCategory = (int)MessageCategory.INFO, MessageText = msg });
                }
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        [Authorize]
        [HttpPost]
        public ActionResult BookFlight(FlightViewModel viewModel)
        {
            try
            {
                var flight = db.FLIGHT_SCHEDULE.Where(f => f.Id == viewModel.FlightId).FirstOrDefault();
                if (flight != null)
                {   
                    if (viewModel.DepartureDate > flight.DepartureDate || viewModel.DepartureDate < flight.DepartureDate)
                    {
                        SetMessage(new Message() { MessageText = "No flight matches your selected departure date", MessageCategory = (int)MessageCategory.ERROR });
                    }
                    else
                    {
                        viewModel.FlightSummary.FlightName = flight.Name;
                        viewModel.FlightSummary.NumberOfTickets = viewModel.NumberOfTickets;
                        viewModel.FlightSummary.PricePerTicket = (decimal)flight.PricePerTicket * viewModel.NumberOfTickets;
                        viewModel.FlightSummary.To = flight.AVAILABLE_FLIGHT_CITIES1.Name;
                        viewModel.FlightSummary.From = flight.AVAILABLE_FLIGHT_CITIES.Name;
                        viewModel.FlightSummary.FlightId = flight.Id;

                        if (viewModel.TripType == 1)
                        {
                            viewModel.IsRoundTrip = true;
                            viewModel.IsOneWayTrip = false;
                            
                            var returnFlight = db.FLIGHT_SCHEDULE.Where(f => f.AVAILABLE_FLIGHT_CITIES.Name.Contains(flight.AVAILABLE_FLIGHT_CITIES1.Name) && f.AVAILABLE_FLIGHT_CITIES1.Name.Contains(flight.AVAILABLE_FLIGHT_CITIES.Name) && f.DepartureDate > DateTime.Now).FirstOrDefault();
                            if (returnFlight != null)
                            {
                                viewModel.FlightSummary.FlightName = returnFlight.Name;
                                viewModel.FlightSummary.NumberOfTickets = viewModel.NumberOfTickets;
                                viewModel.FlightSummary.PricePerTicket = (decimal)returnFlight.PricePerTicket * viewModel.NumberOfTickets;
                                viewModel.FlightSummary.To = returnFlight.AVAILABLE_FLIGHT_CITIES1.Name;
                                viewModel.FlightSummary.From = returnFlight.AVAILABLE_FLIGHT_CITIES.Name;
                                viewModel.FlightSummary.FlightId = returnFlight.Id;
                            }
                        }
                        if (viewModel.TripType == 2)
                        {
                            viewModel.IsRoundTrip = false;
                            viewModel.IsOneWayTrip = true;
                        }
                    }
                }
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        [Authorize]
        public ActionResult CheckAvailablity()
        {
            try
            {
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        [Authorize]
        [HttpPost]
        public ActionResult CheckAvailablity(FlightViewModel viewModel)
        {
            try
            {
                var flights = db.FLIGHT_SCHEDULE.Where(f => f.AVAILABLE_FLIGHT_CITIES.Name.Contains(viewModel.OriginCity.ToUpper()) && f.AVAILABLE_FLIGHT_CITIES1.Name.Contains(viewModel.DestinationCity.ToUpper()) && f.DepartureDate > DateTime.Now).ToList();
                if (flights?.Count() > 0)
                {
                    foreach (var flight in flights)
                    {
                        var flightAvailablity = flight.FLIGHT_AVAILABLITY.Where(f => f.FlightId == flight.Id).FirstOrDefault();

                        var singleFlight = new FlightList()
                        {
                            FlightId = flight.Id,
                            Mileage = flight.Mileage,
                            DepartureDate = flight.DepartureDate.ToString(),
                            FlightName = flight.Name,
                            DestinationCity = flight.AVAILABLE_FLIGHT_CITIES1.Name,
                            OriginCity = flight.AVAILABLE_FLIGHT_CITIES.Name,
                            TotalSeats = flightAvailablity.TotalAvaliableSeats,
                            AvailableSeats = (flightAvailablity.TotalAvaliableSeats - flightAvailablity.CurrentSeatsTaken)
                        };

                        viewModel.FlightList.Add(singleFlight);
                    }
                }
                else
                {
                    //Look for Connecting Flights And then Suggest to the User
                    var connectingFlight = db.FLIGHT_SCHEDULE.Where(f => f.AVAILABLE_FLIGHT_CITIES.Name.Contains(viewModel.OriginCity) && f.DepartureDate > DateTime.Now).ToList();
                    if(connectingFlight?.Count() > 0)
                    {
                        foreach (var flight in connectingFlight)
                        {
                            var flightAvailablity = flight.FLIGHT_AVAILABLITY.Where(f => f.FlightId == flight.Id).FirstOrDefault();

                            var singleFlight = new FlightList()
                            {
                                FlightId = flight.Id,
                                Mileage = flight.Mileage,
                                DepartureDate = flight.DepartureDate.ToString(),
                                FlightName = flight.Name,
                                DestinationCity = flight.AVAILABLE_FLIGHT_CITIES1.Name,
                                OriginCity = flight.AVAILABLE_FLIGHT_CITIES.Name,
                                TotalSeats = flightAvailablity.TotalAvaliableSeats,
                                AvailableSeats = (flightAvailablity.TotalAvaliableSeats - flightAvailablity.CurrentSeatsTaken)
                            };

                            viewModel.ConnectingFlightList.Add(singleFlight);
                        }

                        SetMessage(new Message() { MessageText = "No direct flights were found for your search. Consider taking a connecting flight", MessageCategory = (int)MessageCategory.INFO });
                    }
                    if(viewModel.ConnectingFlightList?.Count() < 1 && viewModel.FlightList?.Count() < 1)
                    {
                        SetMessage(new Message() { MessageText = "No flights were found for your search. Consider searching using the Cities listed below", MessageCategory = (int)MessageCategory.INFO });
                        var availableCities = db.AVAILABLE_FLIGHT_CITIES.Where(c => c.Active).ToList();
                        if (availableCities?.Count() > 0)
                        {
                            List<string> cities = new List<string>();
                            foreach (var item in availableCities)
                            {
                                cities.Add(item.Name);
                            }

                            TempData["ValidCities"] = cities;
                            TempData.Keep();
                        }
                    }
                }
                return View(viewModel);
            }
            catch (Exception ex) { throw ex; }
        }

        [Authorize]
        public ActionResult FinalizeBooking(int flightId, int noOfTickets)
        {
            try
            {
                var user = db.USERs.Where(u => u.Email == User.Identity.Name).FirstOrDefault();

                var flightReservation = new Entity.FLIGHT_RESERVATION()
                {
                    BlockingNumber = Convert.ToString(Guid.NewGuid()),
                    DateCreated = DateTime.Now,
                    FlightId = flightId,
                    UserId = user.Id,
                    Status = "BLOCKED",
                };

                db.FLIGHT_RESERVATION.Add(flightReservation);

                //update user mileage hours
                var flight = db.FLIGHT_SCHEDULE.Where(f => f.Id == flightId).FirstOrDefault();
                if (flight != null)
                {
                    user.FlyMiles += (flight.Mileage * noOfTickets);
                    db.Entry(user).State = EntityState.Modified;

                    var flightAvailability = db.FLIGHT_AVAILABLITY.Where(f => f.FlightId == flightId).FirstOrDefault();
                    if (flightAvailability != null)
                    {
                        flightAvailability.CurrentSeatsTaken += noOfTickets;
                        db.Entry(flightAvailability).State = EntityState.Modified;
                    }
                }
                db.SaveChanges();

                SetMessage(new Message() { MessageText = "Tickets have been 'BLOCKED' for you", MessageCategory = (int)MessageCategory.SUCCESS });

                return RedirectToAction("BookFlight");
            }
            catch(Exception ex) { throw ex; }
        }

        //ConfirmationNumber
        [Authorize]
        [HttpGet]
        public ActionResult ConfirmYourTicket()
        {
            try
            {
                return View();
            }
            catch(Exception ex) { throw ex; }
        }

        //ConfirmationNumber
        [Authorize]
        [HttpPost]
        public ActionResult ConfirmYourTicket(FlightViewModel viewModel)
        {
            try
            {
                if (!string.IsNullOrEmpty(viewModel.BlockingNumber))
                {
                    var flightReservation = db.FLIGHT_RESERVATION.Where(f => f.BlockingNumber.Contains(viewModel.BlockingNumber)).FirstOrDefault();
                    if (flightReservation != null)
                    {
                        flightReservation.Status = "CONFIRMED";
                        db.Entry(flightReservation).State = EntityState.Modified;

                        db.SaveChanges();
                        var message = new Message() { MessageCategory = (int)MessageCategory.SUCCESS, MessageText = "Ticket was confirmed" };
                        SetMessage(message);
                    }
                    else
                    {
                        var message = new Message() { MessageCategory = (int)MessageCategory.ERROR, MessageText = "No ticket was reserved under this number" };
                        SetMessage(message);
                    }
                }
               
                return View(viewModel);
            }
            catch (Exception ex) { throw ex; }
        }

        //Reschedule a ticket

        //Cancel a ticket
    }
}