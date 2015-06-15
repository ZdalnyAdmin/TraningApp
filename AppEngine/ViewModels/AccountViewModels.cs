using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppEngine.Models.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Podaj Login!")]
        [Display(Name = "Login")]
        [StringLength(12, ErrorMessage = "{0} musi mieć długość pomiędzy {2} - {1}.", MinimumLength = 8)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Podaj Hasło!")]
        [StringLength(12, ErrorMessage = "{0} musi mieć długość pomiędzy {2} - {1}.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessage = "Hasła się nie zgadzają.")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }

        public string UserId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Display(Name = "Nazwa Użytkownika")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi posiadać długość większą niż {2}.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Hasła się nie zgadzają.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ChangeEmailViewModel
    {
        [Display(Name = "Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class ChangeUserNameViewModel
    {
        [Display(Name = "Nazwa użytkownika")]
        [StringLength(30, ErrorMessage = "{0} musi mieć długość pomiędzy {2} - {1}.", MinimumLength = 3)]
        [Required]
        public string UserName { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }
    }

    public class CheckUserModel
    {
        public string Id { get; set; }
    }
}
