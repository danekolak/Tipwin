using ExtendedValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;
using Tipwin.Date;

namespace Tipwin.Models
{

    public class Player
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Odaberite način oslovljavanja")]
        public string Oslovljavanje { get; set; }

        [DataType(DataType.Text)]
        [NotEqualTo("KorisnickoIme", ErrorMessage = "Mora biti različito od korisničkog imena")]
        [RegularExpression("([a-zA-Z]{3,30})+", ErrorMessage = "Upišite ime")]
        [Required(ErrorMessage = "Molimo unesite svoje puno ime.Uplate ili isplate bit će uspješno provedene samo u slučaju podudarnosti unesenog imena i prezimena ")]
        public string Ime { get; set; }

        [Required(ErrorMessage = "Molimo unesite prezime...")]
        [DataType(DataType.Text)]
        [NotEqualTo("KorisnickoIme", ErrorMessage = "Mora biti različito od korisničkog imena")]
        [RegularExpression("([a-zA-Z]{3,30})+", ErrorMessage = "Upišite prezime")]
        public string Prezime { get; set; }

        [Required(ErrorMessage = "Molimo unesite datum....")]
        [Display(Name = "Datum rođenja")]
        [ValidateAge(18, 90)]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DatumRodjenja { get; set; }

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

        [Required(ErrorMessage = "Molimo unesite naziv ulice")]
        [RegularExpression(@"^(([a-zA-Z]+[\s]{1}[0-9]{1,5}))$", ErrorMessage = "Upišite naziv ulice (naziv i broj)")]
        public string Ulica { get; set; }
        [BindNever]
        [Display(Name = "Kućni broj")]
        public string KucniBroj { get; set; }

        [Required]
        [Display(Name = "Grad/mjesto")]
        [RegularExpression("([a-zA-Z]{4,30})+", ErrorMessage = "Upišite grad/mjesto")]
        [DataType(DataType.Text)]
        public string GradMjesto { get; set; }

        [Required(ErrorMessage = "Upišite poštanski broj")]
        [Display(Name = "Poštanski broj")]
        [DataType(DataType.PostalCode)]
        [Range(10000, 99999)]
        public int PostanskiBroj { get; set; }

        [Required]
        [Display(Name = "Država")]
        public string Drzava { get; set; }

        [Required]
        [Display(Name = "Jezik za kontakt")]
        public string JezikZaKontakt { get; set; }

        [Display(Order = 9, Name = "Broj telefona")]
        //[RegularExpression("^[01]?[- .]?\\(?[2-9]\\d{2}\\)?[- .]?\\d{3}[- .]?\\d{4}$", ErrorMessage = "Phone is required and must be properly formatted.")]
        [DataType(DataType.PhoneNumber)]
        public int? BrojTelefona { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Broj mobitela")]
        public int? BrojMobitela { get; set; }


        [Required(ErrorMessage = "Korisničko ime je zauzeto")]
        [DataType(DataType.Text)]
        [Display(Name = "Korisničko ime")]
        [MinLength(6), MaxLength(20)]
        [NotEqualTo("Lozinka", ErrorMessage = "Korisničko ime i lozinka ne mogu biti isti")]
        [Remote("UsernameExists", "Player", ErrorMessage = "User Name already in use")]
        [RegularExpression(@"^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Korisničko ime mora imat najmanje 6 slova i može sadržavat brojeve")]
        public string KorisnickoIme { get; set; }

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
}
