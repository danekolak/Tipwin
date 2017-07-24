using ExtendedValidation;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Tipwin.ViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je zauzeto")]
        [DataType(DataType.Text)]
        [Display(Name = "Korisničko ime")]
        [MinLength(6), MaxLength(20)]
        [NotEqualTo("Lozinka", ErrorMessage = "Korisničko ime i lozinka ne mogu biti isti")]
        [Remote("IsUserExists", "Player", ErrorMessage = "User Name already in use")]
        [RegularExpression(@"^([a-zA-Z0-9]{6,20})$", ErrorMessage = "Korisničko ime može sadržavat i brojeve")]
        public string KorisnickoIme { get; set; }

        [Required(ErrorMessage = "Lozinka mora sadržavati velika i mala slova broj")]
        [MinLength(8), MaxLength(40)]
        [RegularExpression(@"^.*(?=.{8,40})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).*$", ErrorMessage = "Lozinka mora sadržavati prvo slova,pa broj i specijalni znak")]
        [NotEqualTo("KorisnickoIme", ErrorMessage = "Lozinka ne može biti ista kao i ime,prezime, korisničko ime")]
        [UIHint("password")]
        [DataType(DataType.Password)]
        public string Lozinka { get; set; }


    }
}
