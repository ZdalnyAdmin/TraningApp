using AppEngine.Helpers;
using AppEngine.Models.DataBusiness;
using AppEngine.Models.DataContext;
using AppEngine.Models.DTO;
using AppEngine.Models.ViewModels.Account;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
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

        public ProfileEnum Profile { get; set; }
        public string DisplayName { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime InvitationDate { get; set; }
        public DateTime? LastActivationDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string DeleteUserID { get; set; }
        public int? OrganizationID { get; set; }
        public Organization Organization { get; set; }
        public DateTime? ResetPasswordDate { get; set; }
        public string InviterID { get; set; }
        public Person Inviter { get; set; }
        public int? DailyChangeMailCount { get; set; }

        public string NewUserName { get; set; }
        public DateTime? ChangeUserNameDate { get; set; }

        public string NewEmail { get; set; }
        public DateTime? ChangeEmailDate { get; set; }

        public DateTime? DeleteUserDate { get; set; }

        [NotMapped]
        public string ModifiedUserID { get; set; }
        //public List<TrainingResult> AssignedTrainings { get; set; }

        [NotMapped]
        public int TrainingNumber { get { return AssignedTrainings == null ? 0 : AssignedTrainings.Count(); } }

        [NotMapped]
        public List<ProfileGroup2Person> AssignedGroups { get; set; }

        [NotMapped]
        public List<TrainingDto> AssignedTrainings { get; set; }

        #endregion Properties

        #region Methods
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Person> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserManager<Person> manager, HttpRequestBase request, bool sendMail = true)
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

        public async Task<IdentityResult> ChangeEmailAsync(UserManager<Person> manager, HttpRequestBase request, string newEmail)
        {
            var result = await manager.UpdateSecurityStampAsync(this.Id);

            if (!result.Succeeded)
            {
                return result;
            }

            var code = await manager.GenerateUserTokenAsync("CHANGE_EMAIL", this.Id);

            this.ChangeEmailDate = DateTime.Now;
            this.NewEmail = newEmail;
            result = await manager.UpdateAsync(this);

            if (!result.Succeeded)
            {
                return result;
            }

            MailMessage mail = new MailMessage(new MailAddress(Helpers.Helpers.GetMailFrom(MailAccount.ADMIN), "Kenpro"),
                                   new MailAddress(newEmail))
            {
                Subject = "Zmiana Adresu Email",
                Body = "Została wysłana prośba o zmianę twojego maila na " + newEmail + ". <br/>"
                + "Jeżeli chcesz zmienić adres email potwierdź to wciskając link. <br/>"
                + "<a href=\""
                + request.Url.Scheme + "://" + request.Url.Authority + "/changeEmail?code=" + code + "&Id=" + this.Id + "\">link</a>",
                IsBodyHtml = true
            };

            mail.BodyEncoding = UTF8Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            Mail.Send(mail, MailAccount.ADMIN);

            return result;
        }

        public async Task<IdentityResult> DeleteUserAsync(UserManager<Person> manager, HttpRequestBase request, string deleteUser = null)
        {
            var result = await manager.UpdateSecurityStampAsync(this.Id);

            if (!result.Succeeded)
            {
                return result;
            }

            var code = await manager.GenerateUserTokenAsync("DELETE_USER", this.Id);

            this.DeleteUserDate = DateTime.Now;
            result = await manager.UpdateAsync(this);

            if (!result.Succeeded)
            {
                return result;
            }

            var sb = new StringBuilder();
            if (deleteUser != null)
            {
                sb.AppendFormat("<b>ZOSTAŁA WYSŁANA PROŚBA O USUNIĘCIE UŻYTKOWNIKA </b> {0}", this.DisplayName);
                sb.AppendLine();
                sb.AppendLine("JEŚLI CHCESZ GO USUNAĆ POTWIERDZ TO WCISKAJĄC LINK");
                sb.AppendLine();
            }
            else
            {
                var organization = _db.Organizations.FirstOrDefault(x => x.OrganizationID == this.OrganizationID);

                sb.AppendFormat("<b>WITAJ </b> {0}", this.DisplayName);
                sb.AppendLine();
                sb.AppendFormat("ROZPOCZĄŁEŚ PRÓBĘ USUNIECIA SWOJEGO KONTA W ORGANIZACJI {0}", organization != null ? organization.Name : "Brak Nazwy");
                sb.AppendLine();
                sb.AppendLine("JEŚLI CHCESZ USUNĄĆ SWOJE KONTO TO POTWIERDŹ TO KLIKAJĄC W LINK");
                sb.AppendLine();
            }

            sb.AppendFormat("<br/><a href=\"{0}://{1}/deleteUser?Id={2}&code={3}\">LINK</a>", request.Url.Scheme, request.Url.Authority, this.Id, code);

            await manager.SendEmailAsync(deleteUser != null ? deleteUser : this.Id,
               "POTWIERDZENIE USUNIECIA UZYTKOWNIKA",
               sb.ToString());

            return result;
        }

        public static async Task<Result> ChangePasswordAsync(UserManager<Person> manager, ResetPasswordViewModel model, bool sendMailToProtector = true)
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

            EFContext db = new EFContext();
            var organization = db.Organizations.FirstOrDefault(x => x.OrganizationID == userByUserName.OrganizationID);

            if (sendMailToProtector)
            {
                await manager.SendEmailAsync(userByUserName.InviterID, "Zmiana hasała przez podopiecznego",
                    string.Format("Użytkownik organizacji {0} i o Id {1} i nazwie wyświetlania {2} zmienił swoje hasło.", organization.Name, userByUserName.Id, userByUserName.DisplayName));
            }

            await manager.UpdateSecurityStampAsync(userByUserName.Id);

            return new Result() { Succeeded = resetResult.Succeeded, Errors = new List<string>(resetResult.Errors) };
        }

        public static Person GetLoggedPerson(IPrincipal User, EFContext dbContext = null)
        {
            EFContext db = dbContext;

            if(db == null)
                db = new EFContext();

            string currentUserId = User.Identity.GetUserId();

            if (string.IsNullOrWhiteSpace(currentUserId))
            {
                return null;
            }

            var user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            if (user == null)
            {
                return null;
            }
            user.SetAssignedTrainingsNumber((from t in db.TrainingResults
                                             where t.PersonID == user.Id
                                             select t).Count());

            var groups = (from pg in db.PeopleInGroups
                          join g in db.Groups on pg.ProfileGroupID equals g.ProfileGroupID
                          where pg.PersonID == user.Id
                          select g.Name).ToList();

            user.SetAssignedGroups(groups);
            user.Organization = db.Organizations.FirstOrDefault(x => x.OrganizationID == user.OrganizationID);

            return user;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public void SetAssignedTrainingsNumber(int number)
        {
            _trainingNumber = number;
        }

        public void SetAssignedGroups(List<string> groups)
        {
            if (groups != null && groups.Any())
            {
                _assignedGroups = String.Join(",", groups);
            }
        }
        #endregion Methods
    }
}
