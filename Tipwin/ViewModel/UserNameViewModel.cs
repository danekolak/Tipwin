using ExtendedValidation;
using System;
using System.ComponentModel.DataAnnotations;
using Tipwin.Date;

namespace Tipwin.ViewModel
{
    public class UserNameViewModel
    {
        [Required(ErrorMessage = "Molimo unesite el. poštu...")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [NotEqualTo("Lozinka", ErrorMessage = "Mora biti različito od lozinke")]
        [Display(Name = "El.pošta")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Molimo unesite datum....")]
        [Display(Name = "Datum rođenja")]
        [ValidateAge(18, 90)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }

    }
}
