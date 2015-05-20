using AppEngine.Models.Common;
using AppEngine.Models.DataBusiness;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace AppEngine.Helpers
{
    public static class Helpers
    {
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static int GetUserID(Person person)
        {
            if (person == null)
            {
                return 0;
            }

            int id = 0;

            int.TryParse(person.Id, out id);

            return id;
        }
        public static bool CheckAccess(MethodBase method, ProfileEnum profile)
        {
            var attributes = method.GetCustomAttributes(typeof(AccessAttribute), true);

            if (attributes.Count() == 0)
            {
                return true;
            }

            var access = attributes.FirstOrDefault() as AccessAttribute;

            if (access == null)
            {
                return true;
            }

            switch (access.MinimumProfile)
            {
                case ProfileEnum.Superuser:
                    return profile == ProfileEnum.Superuser;

                case ProfileEnum.Protector:
                    return profile == ProfileEnum.Superuser ||
                           profile == ProfileEnum.Protector;

                case ProfileEnum.Administrator:
                    return profile == ProfileEnum.Superuser ||
                           profile == ProfileEnum.Administrator ||
                           profile == ProfileEnum.Protector;

                case ProfileEnum.Manager:
                    return profile == ProfileEnum.Superuser ||
                           profile == ProfileEnum.Administrator ||
                           profile == ProfileEnum.Manager ||
                           profile == ProfileEnum.Protector;

                case ProfileEnum.Creator:
                    return profile == ProfileEnum.Superuser ||
                           profile == ProfileEnum.Administrator ||
                           profile == ProfileEnum.Creator ||
                           profile == ProfileEnum.Manager ||
                           profile == ProfileEnum.Protector;

                case ProfileEnum.User:
                default:
                    return true;
            }
        }
    }
}
