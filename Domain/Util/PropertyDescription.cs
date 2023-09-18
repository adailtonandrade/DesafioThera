using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Util
{

    public static class PropertyDescription
    {
        /* This method get name the of annotation "Description" */
        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetEnumDisplayName<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DisplayAttribute[] attributes =
                (DisplayAttribute[])fi.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length == 0)
                return null;
            
            return (attributes[0] as DisplayAttribute).GetName();

        }

        public static string GetAttributeDisplayName(PropertyInfo property)
        {
            var atts = property.GetCustomAttributes(
                typeof(DisplayAttribute), true);
            if (atts.Length == 0)
                return null;
            return (atts[0] as DisplayAttribute).GetName();
        }
    }
}
