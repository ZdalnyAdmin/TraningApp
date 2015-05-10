using AppEngine.Models.Common;
using System;
using System.ComponentModel;
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
    }
}
