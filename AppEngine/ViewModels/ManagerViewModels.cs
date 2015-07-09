using AppEngine.Models.DataBusiness;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEngine.Models.ViewModels.Manager
{
    public class InviteUserViewModel
    {
        [Required]
        [Display(Name = "Nazwa Użytkownika")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Adres email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Rola")]
        public ProfileEnum Role { get; set; }
    }

    public class RemoveInvitationModel
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }
    }
}
