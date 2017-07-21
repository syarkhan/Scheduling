using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Models.ViewModels
{
    public class RegisterVM
    {

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        [Remote("UsernameValidate", "JSON", ErrorMessage = "Username already exists")]
        public string Username { get; set; }



        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        [StringLength(15, MinimumLength = 8, ErrorMessage = "Password should be between 8 to 15 characters")]
        [RegularExpression(@"^\S*$", ErrorMessage = "White space is not allowed")]
        public string Password { get; set; }


        [Required(ErrorMessage = "Confirm Password is required")]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        
        public string ConfirmPassword { get; set; }

    }
}