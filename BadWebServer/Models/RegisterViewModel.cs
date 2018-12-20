using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GoodServer.Models
{
    public class RegisterViewModel
    {
        [EmailAddress(ErrorMessage = "Needs to be a valid Email Address")]
        [Required]
        [Display(Name ="Your Email Address")]
        public string Email { get; set; }


        [MinLength(8,ErrorMessage = "Your password must be at least 8 characters long")]
        [Required]
        [Display(Name ="Your Password")]
        public string Password { get; set; }

    }
}
