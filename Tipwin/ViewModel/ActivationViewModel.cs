using System.ComponentModel.DataAnnotations;
namespace Tipwin.ViewModel
{
    public class ActivationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Upišite aktivacijski kod")]
        public string ActivationCode { get; set; }

        public int PlayersId { get; set; }
    }
}
