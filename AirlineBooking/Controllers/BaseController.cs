using AirlineBooking.Entity;
using AirlineBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AirlineBooking.Controllers
{
    public class BaseController : Controller
    {
        protected ARS_AIRLINESEntities1 db;

        public BaseController()
        {
            db = new ARS_AIRLINESEntities1();
        }

        public void SetMessage(Message message)
        {
            TempData["Message"] = message;
            TempData.Keep();
        }

        public USER GetUserByEmail(string email)
        {
            try
            {
                USER user = db.USERs.Where(u => u.Email == email).FirstOrDefault();
                return user;
            }
            catch (Exception ex) { throw ex; }
        }

        public USER GetUserByCreditCardNumber(string creditCardNumber)
        {
            try
            {
                USER user = db.USERs.Where(u => u.CreditCardNumber == creditCardNumber).FirstOrDefault();
                return user;
            }
            catch (Exception ex) { throw ex; }
        }


        public USER GetUserById(int Id)
        {
            try
            {
                USER user = user = db.USERs.Where(u => u.Id == Id).FirstOrDefault();
                return user;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}