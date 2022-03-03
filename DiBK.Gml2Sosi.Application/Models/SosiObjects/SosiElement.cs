using DiBK.Gml2Sosi.Application.Attributes;
using System.Reflection;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiElement
    {
        public virtual string ElementName { get; }
        public List<string> SosiValues { get; } = new();

        public virtual void WriteToStream(StreamWriter streamWriter)
        {
        }

        protected virtual void SetSosiValues()
        {
            SosiValues.Clear();
            SetSosiValues(this);
        }

        private void SetSosiValues(object obj)
        {
            var props = obj.GetType().GetProperties()
                .Where(prop => Attribute.IsDefined(prop, typeof(SosiPropertyAttribute)))
                .OrderBy(prop => prop.GetCustomAttribute<SosiPropertyAttribute>().Order);

            foreach (var prop in props)
            {
                var value = prop.GetValue(obj, null);
                var attribute = prop.GetCustomAttribute<SosiPropertyAttribute>();

                if (value is string stringValue)
                {
                    SosiValues.Add($"{attribute.SosiName} {stringValue}");
                }
                else if (value is not null)
                {
                    SosiValues.Add(attribute.SosiName);
                    SetSosiValues(value);
                }
            }
        }
    }
}
