using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Scheduling.Models.ViewModels
{
    public class AddClassVM
    {
        [Required(ErrorMessage = "Please select an offered course")]
        public int[] offids { get; set; }

        public int occupied { get; set; }

        public int dayid { get; set; }

        public int slottypeid { get; set; }

        public int roomid { get; set; }
        
        public string start { get; set; }

    }
}