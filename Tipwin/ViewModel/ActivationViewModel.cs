using System.ComponentModel.DataAnnotations;
namespace Tipwin.ViewModel
{
    public class ActivationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Upišite aktivacijski kod")]
        public string ActivationCode { get; set; }
        public string Email { get; set; }
        public string EmailPonovo { get; set; }

        public int PlayersId { get; set; }

        public bool Provjeren { get; set; }
    }
}
