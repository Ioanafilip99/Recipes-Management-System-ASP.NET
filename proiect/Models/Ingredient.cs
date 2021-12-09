using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [MinLength(2, ErrorMessage = "Ingredient name cannot be less than 2!"),
           MaxLength(100, ErrorMessage = "Ingredient name cannot be more than 100!")]
        public string Name { get; set; }

        // many-to-many relationship
        public virtual ICollection<Recipe> Recipes { get; set; }

    }
}