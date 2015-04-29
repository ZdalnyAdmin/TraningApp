using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace AppEngine.Models.Common
{
    public class Person : IdentityUser
    {
        private EFContext _db = new EFContext();
        private int _trainingNumber;
        private string _assignedGroups;

        #region Properties

        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int StatusID { get; set; }
        public Status Status { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int RegistrationUserID { get; set; }
        public DateTime? LastActivationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeleteUserID { get; set; }
        public int? OrganizationID { get; set; }
        public Organization Organization { get; set; }
        public DateTime? ResetPasswordDate { get; set; }

        [NotMapped]
        public string ModifiedUserID { get; set; }
        //public List<TrainingResult> AssignedTrainings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TrainingNumber
        {
            get { return _trainingNumber; }
        }

        public string AssignedGroups
        {
            get { return _assignedGroups; }
        }

        #endregion Properties

        #region Methods
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Person> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserManager<Person> manager, HttpRequestBase request)
        {
            var result = await manager.UpdateSecurityStampAsync(this.Id);

            if (!result.Succeeded)
            {
                return result;
            }

            var code = await manager.GeneratePasswordResetTokenAsync(this.Id);
            
            this.ResetPasswordDate = DateTime.Now;
            result = await manager.UpdateAsync(this);

            if (!result.Succeeded)
            {
                return result;
            }

            await manager.SendEmailAsync(this.Id, "Zmiana Hasła",
            "Zostało wysłane zgłoszenia zmiany hasła, aby kontynuować kliknij w <a href=\""
                + request.Url.Scheme + "://" + request.Url.Authority + "/resetPasswordConfirmation?code=" + code + "\">link</a>");

            return result;
        }

        public static async Task<Result> ChangePasswordAsync(UserManager<Person> manager, ResetPasswordViewModel model)
        {
            var userByUserName = await manager.FindByNameAsync(model.UserName);
            if (userByUserName == null)
            {
                var errors = new List<string>();
                errors.Add("Błędna nazwa użytkownika.");

                return new Result()
                {
                    Succeeded = false,
                    Errors = errors
                };
            }

            if (!userByUserName.ResetPasswordDate.HasValue || (DateTime.Now - userByUserName.ResetPasswordDate.Value).Days > 0)
            {
                var errors = new List<string>();
                errors.Add("Token wygasł.");

                return new Result()
                {
                    Succeeded = false,
                    Errors = errors
                };
            }

            var resetResult = await manager.ResetPasswordAsync(userByUserName.Id, model.Code, model.Password);

            if (!resetResult.Succeeded)
            {
                return new Result() { Succeeded = resetResult.Succeeded, Errors = new List<string>(resetResult.Errors) }; ;
            }

            await manager.UpdateSecurityStampAsync(userByUserName.Id);

            return new Result() { Succeeded = resetResult.Succeeded, Errors = new List<string>(resetResult.Errors) };
        }

        public override string ToString()
        {
            return Name;
        }

        public void SetAssignedTrainingsNumber(int number)
        {
            _trainingNumber = number;
        }

        public void SetAssignedGroups(List<string> groups)
        {
            if(groups != null && groups.Any())
            {
                _assignedGroups = String.Join(",", groups);
            }
        }

        #endregion Methods
    }
}
