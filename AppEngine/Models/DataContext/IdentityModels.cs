using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;

namespace AppEngine.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserManager<ApplicationUser> manager, HttpRequestBase request)
        {
            var result = await manager.UpdateSecurityStampAsync(this.Id);

            if (!result.Succeeded)
            {
                return result;
            }

            var code = await manager.GeneratePasswordResetTokenAsync(this.Id);
            await manager.SendEmailAsync(this.Id, "Zmiana Hasła",
            "Zostało wysłane zgłoszenia zmiany hasła, aby kontynuować kliknij w <a href=\""
                + request.Url.Scheme + "://" + request.Url.Authority + "/resetPasswordConfirmation?code=" + code + "\">link</a>");

            return result;
        }
    }
}