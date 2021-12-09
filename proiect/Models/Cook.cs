using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class Cook
    {
        public int CookId { get; set; }

        [RegularExpression("^[A-Z]+(([' -][a-zA-Z ])?[a-zA-Z]*)*$",
            ErrorMessage = "Invalid name!")]
        [MinLength(2, ErrorMessage = "Cook name cannot be less than 2!"),
           MaxLength(100, ErrorMessage = "Cook name cannot be more than 100!")]
        public string Name { get; set; }

        // many-to-one relationship
        public virtual ICollection<Recipe> Recipes { get; set; }

        // one-to-one relationship
        [Required]
        public virtual ContactInfo ContactInfo { get; set; }
    }
}