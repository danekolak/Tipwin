using ExtendedValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tipwin.Date;

namespace Tipwin.ViewModel
{
    public class UserNameViewModel
    {
        [Required(ErrorMessage = "Molimo unesite email adresu...")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [NotEqualTo("Lozinka", ErrorMessage = "Mora biti različito od lozinke")]
        [Display(Name = "Email adresa")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Molimo unesite datum....")]
        [Display(Name = "Datum rođenja")]
        [ValidateAge(18, 90)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }

    }


    public class PasswordViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je zauzeto")]
        [DataType(DataType.Text)]
        [Display(Name = "Korisničko ime")]
        [MinLength(6), MaxLength(20)]
        [NotEqualTo("Lozinka", ErrorMessage = "Korisničko ime i lozinka ne mogu biti isti")]
        [Remote("UsernameExists", "Player", ErrorMessage = "User Name already in use")]
        [RegularExpression(@"^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Korisničko ime mora imat najmanje 6 slova i može sadržavat brojeve")]
        public string KorisnickoIme { get; set; }


        [Required(ErrorMessage = "Molimo unesite datum....")]
        [Display(Name = "Datum rođenja")]
        [ValidateAge(18, 90)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }

    }

    public class ChangePasswordViewModel
    {

        [Required(ErrorMessage = "Lozinka mora sadržavati velika i mala slova broj")]
        [MinLength(8), MaxLength(40)]
        [RegularExpression(@"^.*(?=.{8,40})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$", ErrorMessage = "Lozinka mora sadržavati prvo slova,pa broj i specijalni znak")]
        [NotEqualTo("KorisnickoIme", ErrorMessage = "Lozinka ne može biti ista kao i ime,prezime, korisničko ime")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }



        [Required(ErrorMessage = "Lozinka nije ista")]
        [RegularExpression("^[a-z0-9A-Z!&=%_:;~@_#$?{}|+,^.-]{8,40}$", ErrorMessage = "Lozinka mora sadržavati velika i mala slova broj")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        [MinLength(8), MaxLength(40)]
        [System.ComponentModel.DataAnnotations.Compare("Lozinka", ErrorMessage = "Lozinka nije ista")]
        [Display(Name = "Ponovite lozinku")]
        public string LozinkaPonovo { get; set; }

    }

    public class ChangeEmailViewModel
    {
        [Required(ErrorMessage = "Molimo unesite email adresu...")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [NotEqualTo("Lozinka", ErrorMessage = "Mora biti različito od lozinke")]
        [Display(Name = "Email adresa")]
        public string Email { get; set; }



        [Required(ErrorMessage = "Molimo potvrdite email adresu....")]
        [System.ComponentModel.DataAnnotations.Compare("Email", ErrorMessage = "Email adresa nije ista")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3]\.)|(([\w-]+\.)+))([a-zA-Z{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Unesite ispravnu el. poštu")]
        [Display(Name = "Ponovite email adresu")]
        [DataType(DataType.EmailAddress)]
        public string EmailPonovo { get; set; }
    }


}
