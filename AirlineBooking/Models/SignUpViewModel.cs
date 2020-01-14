using AirlineBooking.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AirlineBooking.Models
{
    public class SignUpViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Sex { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public int Age { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [Display(Name ="Credit Card Number")]
        [DataType(DataType.CreditCard, ErrorMessage = "Invalid credit card number")]
        public string CreditCardNumber { get; set; }

        //Select Lists
        public List<Value> SexList { get; set; } = new List<Value>();

        public SignUpViewModel()
        {
            SetSexList();
        }

        
    }

    public class Message
    {
        public string MessageText { get; set; }

        public int MessageCategory { get; set; }
    }

    public enum MessageCategory
    {
        SUCCESS = 1,
        ERROR = 2,
        WARNING = 3,
        DANGER = 4,
        INFO = 5
    };
}