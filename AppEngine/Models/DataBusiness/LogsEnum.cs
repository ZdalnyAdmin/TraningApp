using System.ComponentModel;

namespace AppEngine.Models.DataBusiness
{
    public enum OperationLog
    {
        [Description("Zaproszenie uzytkownika")]
        UserInvitation = 0,
        [Description("Samousuniecie uzytkownika")]
        UserDeleteBySelf = 1,
        [Description("Usuniecie uzytkownika")]
        UserDelete = 2,
        [Description("Utworzenie kursu")]
        TrainingCreate = 3,
        [Description("Utworzenie uzytkownika")]
        UserCreate = 4,
        [Description("Edycja uzytkownika")]
        UserEdit = 5,
        [Description("Edycja szkolenia")]
        TrainingEdit = 6,
        [Description("Usunięcie zaproszenia")]
        InvitationRemove = 7,
        [Description("Rejestracja użytkownika")]
        UserRegistration = 8
    }

    public enum SystemLog
    {

        [Description("Utworznie firmy")]
        OrganizationCreate = 1,

        [Description("Prośba o usunięcie firmy")]
        OrganizationRequestToRemove = 2,

        [Description("Usunięcie firmy")]
        OrganizationDelete = 3,

        [Description("Wysłanie zaproszenia do opiekuna")]
        ProtectorInvitation = 4,

        [Description("Utworzenie opiekuna")]
        ProtectorCreate = 5,

        [Description("Logowanie do panelu systemowego")]
        LogIn = 6,

        [Description("Wylogowanie z panelu systemowego")]
        LogOut = 7

    }

    public class EnumData
    {
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
