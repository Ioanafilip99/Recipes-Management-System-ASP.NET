using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class CookContactViewModel
    {
        [RegularExpression("^[A-Z]+(([' -][a-zA-Z ])?[a-zA-Z]*)*$",
            ErrorMessage = "Invalid name!")]
        [MinLength(2, ErrorMessage = "Cook name cannot be less than 2!"),
       MaxLength(30, ErrorMessage = "Cook name cannot be more than 30!")]
        public string Name { get; set; }

        [RegularExpression(@"^07(\d{8})$",
           ErrorMessage = "This is not a valid phone number!")]
        public string PhoneNumber { get; set; }

        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            ErrorMessage = "This is not a valid email address!")]
        public string Email { get; set; }

        [Range(1, int.MaxValue,
            ErrorMessage = "Please enter a value bigger than 0!")]
        public int Age { get; set; }

        [RegularExpression(@"(^Male|^Female)$",
            ErrorMessage = "This is not a valid gender type!")]
        [Required]
        public Gender GenderType { get; set; }
    }
}