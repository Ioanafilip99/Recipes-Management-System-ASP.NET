using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace proiect.Models.MyValidator
{
    public class CookTimeValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var recipe = (Recipe)validationContext.ObjectInstance;
            int cookTime = recipe.CookTime;
            bool cond = true;
            if(cookTime < 15 || cookTime > 300)
            {
                cond = false;
            }

            return cond ? ValidationResult.Success : cookTime < 15 ? new ValidationResult("Cook time must be at least 15 minutes!") : new ValidationResult("Cook time cannot be more than 5 hours (300 minutes)!");
        }
            
    }
}