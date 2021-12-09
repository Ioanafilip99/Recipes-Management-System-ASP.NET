using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class RecipeType
    {
        public int RecipeTypeId { get; set; }

        [MinLength(2, ErrorMessage = "Recipe type name cannot be less than 2!"),
            MaxLength(100, ErrorMessage = "Recipe type name cannot be more than 100!")]
        public string Name { get; set; }

        // many to one
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}