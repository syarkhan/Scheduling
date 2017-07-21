using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scheduling.Models.ViewModels
{
    public class LoginVM
    {
        
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please Enter Username")]
        public string Username { get; set; }


        
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }

        
        public int Account_Id { get; set; }




    }
}