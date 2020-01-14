using AirlineBooking.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace AirlineBooking.Controllers
{
    public class HomeController : BaseController
    {
        SignUpViewModel viewModel;

        public HomeController()
        {
            viewModel = new SignUpViewModel();
        }

        public ActionResult Index()
        {   
           return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Controller method GET for Signing up a new passenger
        /// </summary>
        /// <returns>View</returns>
        [HttpGet]
        public ActionResult SignUp()
        {
            try
            {   
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        /// <summary>
        /// Controller method POST for Signing up a new passenger
        /// </summary>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult SignUp(SignUpViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (viewModel.Sex < 1)
                    {
                        SetMessage(new Message() { MessageText = "Invalid sex selection", MessageCategory = (int)MessageCategory.DANGER });
                    }
                    else
                    {
                        if (GetUserByEmail(viewModel.Email) != null)
                        {
                            SetMessage(new Message() { MessageText = "This Email is already taken", MessageCategory = (int)MessageCategory.DANGER });
                        }
                        if (GetUserByCreditCardNumber(viewModel.CreditCardNumber) != null)
                        {
                            SetMessage(new Message() { MessageText = "This Credit card is already taken", MessageCategory = (int)MessageCategory.DANGER });
                        }
                        else
                        {
                            var user = new Entity.USER()
                            {
                                FirstName = viewModel.FirstName,
                                LastName = viewModel.LastName,
                                Password = viewModel.Password,
                                Address = viewModel.Address,
                                PhoneNumber = viewModel.PhoneNumber,
                                Email = viewModel.Email,
                                SexId = viewModel.Sex,
                                Age = viewModel.Age,
                                CreditCardNumber = viewModel.CreditCardNumber,
                                FlyMiles = 0,
                                DateCreated = DateTime.Now
                            };

                            db.USERs.Add(user);

                            db.SaveChanges();

                            SetMessage(new Message() { MessageText = "User added successfully", MessageCategory = (int)MessageCategory.SUCCESS });
                            return RedirectToAction("SignUp");
                        }
                    }
                }
                else
                {
                    SetMessage(new Message() { MessageText = "Model Is Invalid", MessageCategory = (int)MessageCategory.DANGER });
                }
                return View(viewModel);
            }
            catch (Exception ex) { throw ex; }
        }

        [HttpGet]
        public ActionResult EditProfile(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    var user = GetUserById(Id);
                    if (user != null)
                    {
                        viewModel.PhoneNumber = user.PhoneNumber;
                        viewModel.CreditCardNumber = user.CreditCardNumber;
                        viewModel.Address = user.Address;
                        viewModel.UserId = user.Id;
                    }
                }

                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        [HttpPost]
        public ActionResult EditProfile(SignUpViewModel viewModel)
        {
            try
            {
                ModelState.Remove("FirstName");
                ModelState.Remove("LastName");
                ModelState.Remove("Email");
                ModelState.Remove("Sex");
                ModelState.Remove("Password");
                ModelState.Remove("Age");

                if (ModelState.IsValid)
                {
                    Entity.USER user = GetUserById(viewModel.UserId);
                    if (user !=  null)
                    {
                        user.Address = viewModel.Address;
                        user.CreditCardNumber = viewModel.CreditCardNumber;
                        user.PhoneNumber = viewModel.PhoneNumber;

                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();

                        SetMessage(new Message() { MessageText = "Profile was edited succesfully", MessageCategory = (int)MessageCategory.SUCCESS });
                    }
                }
                return View(viewModel);
            }
            catch(Exception ex) { throw ex; }
        }

        public ActionResult Login()
        {
            try
            {
                return View();
            }
            catch(Exception ex) { throw ex; }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            try
            {
                var user = db.USERs.Where(u => u.Email == model.Username && u.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, false);
                    return RedirectToAction("Index", "Flight");
                }
                else
                {
                    SetMessage(new Message() { MessageText = "Wrong Credentials", MessageCategory = (int)MessageCategory.ERROR });
                }
                return View(model);
            }
            catch(Exception ex) { throw ex; }
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}