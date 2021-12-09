using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class ContactInfo
    {
        [Key]
        public int ContactInfoId { get; set; }

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
        public Gender GenderType{ get; set; }

        // one-to-one relationship
        public virtual Cook Cook { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}