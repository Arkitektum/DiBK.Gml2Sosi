using DiBK.Gml2Sosi.Application.Attributes;
using System.Reflection;
using System.Text;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiElement
    {
        public virtual string ElementName { get; }
        public int SequenceNumber { get; set; }
        public List<string> SosiValues { get; } = new();

        public virtual Task WriteToStreamAsync(StreamWriter streamWriter)
        {
            return Task.CompletedTask;
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

        public static async Task<MemoryStream> WriteAllToStreamAsync(List<SosiElement> sosiElements)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);

            foreach (var element in sosiElements)
                await element.WriteToStreamAsync(streamWriter);

            await streamWriter.WriteLineAsync(".SLUTT");
            await streamWriter.FlushAsync();
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
