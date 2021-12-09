using proiect.Models.MyValidator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiect.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        [RegularExpression(@"^[A-Za-z ]+$",
            ErrorMessage = "Title must be only letters!")]
        [MinLength(2, ErrorMessage = "Recipe title cannot be less than 2!"),
           MaxLength(1000, ErrorMessage = "Recipe title cannot be more than 1000!")]
        public string Title { get; set; }

        [MinLength(2, ErrorMessage = "Recipe description cannot be less than 2!"),
            MaxLength(200000, ErrorMessage = "Recipe description cannot be more than 20000!")]
        public string Description { get; set; }

        [MinLength(2, ErrorMessage = "Recipe steps cannot be less than 2!"),
            MaxLength(500000, ErrorMessage = "Recipe steps cannot be more than 50000!")]
        public string Steps { get; set; }

        [CookTimeValidator]
        public int CookTime { get; set; }

        // one to many
        public int CookId { get; set; }
        public virtual Cook Cook { get; set; }

        // one to many
        [ForeignKey("RecipeType")]
        public int RecipeTypeId { get; set; }
        public virtual RecipeType RecipeType { get; set; }

        // many to many
        public virtual ICollection<Ingredient> Ingredients { get; set; }

        // dropdown lists
        [NotMapped]
        public IEnumerable<SelectListItem> RecipeTypesList { get; set; }
        public IEnumerable<SelectListItem> CooksList { get; set; }
 
        // checkboxes list
        [NotMapped]
        public List<CheckBoxViewModel> IngredientsList { get; set; }
    }


}