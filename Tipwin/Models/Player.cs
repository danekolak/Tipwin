using ExtendedValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Tipwin.Models
{
    [Serializable]
    public class Player
    {
        [BindNever]
        public int Id { get; set; }
        [Required(ErrorMessage = "Odaberite način oslovljavanja")]
        public string Oslovljavanje { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Molimo unesite svoje puno ime.Uplate ili ispalte bit će uspješno provedene samo u slučaju podudarnosti unesenog imena i prezimena ")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Molimo unesite prezime...")]
        [DataType(DataType.Text)]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Molimo unesite datum....")]
        [Display(Name = "Datum rođenja")]
        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }


        [Required(ErrorMessage = "Molimo unesite el. poštu...")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "El.pošta")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Molimo potvrdite el poštu....")]
        // [Compare("Email", ErrorMessage = "El pošta adresa nije ista")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please enter a valid e-mail adress")]
        [Display(Name = "Ponovite el. poštu")]
        [DataType(DataType.EmailAddress)]
        public string EmailPonovo { get; set; }

        [Required(ErrorMessage = "Molimo unesite naziv ulice")]
        public string Ulica { get; set; }
        [BindNever]
        [Display(Name = "Kućni broj")]
        public string KucniBroj { get; set; }

        [Required]
        [Display(Name = "Grad/mjesto")]
        [DataType(DataType.Text)]
        public string GradMjesto { get; set; }

        [Required]
        [Display(Name = "Poštanski broj")]
        [DataType(DataType.PostalCode)]
        public int PostanskiBroj { get; set; }

        [Required]
        [Display(Name = "Država")]
        public string Drzava { get; set; }

        [Required]
        [Display(Name = "Jezik za kontakt")]
        public string JezikZaKontakt { get; set; }

        [Display(Name = "Broj telefona")]
        [DataType(DataType.PhoneNumber)]
        public int? BrojTelefona { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Broj mobitela")]
        public int? BrojMobitela { get; set; }


        //za korisnicko ime regular expression testing       
        //[RegularExpression("^[a-zA-Z,0-9,-i_ ]*$")]
        //[RegularExpression("^[a-z0-9A-Z!#$?{}|+,^.-+&=%_:;~@]{8,40}$")]
        //+&=%_:;~@]{8,40}$")] ovaj dio javlja gresku   
        //[RegularExpression("[^d{5}(-d{4})?$]")]

        [Required]
        [Display(Name = "Korisničko ime")]
        [MinLength(6), MaxLength(20)]
        [NotEqualTo("Lozinka", ErrorMessage = "Korisničko ime i lozinka ne mogu biti isti")]
        [Remote("IsUserNameAvailable", "Player", ErrorMessage = "Korisničko ime je zauzeto")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka mora sadržavati velika i mala slova broj")]
        [MinLength(8), MaxLength(40)]
        [RegularExpression("^[a-z0-9A-Z!&=%_:;~@_#$?{}|+,^.-]{8,40}$")]
        [NotEqualTo("KorisnickoIme", ErrorMessage = "Lozinka ne može biti ista kao i korisničko ime")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }


        //  [Compare("Lozinka", ErrorMessage = "Lozinka nije ista")]
        [Required(ErrorMessage = "Lozinka nije ista")]
        [RegularExpression("^[a-z0-9A-Z!&=%_:;~@_#$?{}|+,^.-]{8,40}$")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        [MinLength(8), MaxLength(40)]
        [Display(Name = "Ponovite lozinku")]
        public string LozinkaPonovo { get; set; }


    }
}
