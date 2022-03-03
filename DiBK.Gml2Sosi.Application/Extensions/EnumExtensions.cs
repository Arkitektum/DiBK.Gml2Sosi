using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Extensions
{
    public static class EnumExtensions
    {
        public static string GetSosiName(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes = (SosiPropertyAttribute[])fieldInfo.GetCustomAttributes(typeof(SosiPropertyAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].SosiName;
            else
                return value.ToString();
        }
    }
}
