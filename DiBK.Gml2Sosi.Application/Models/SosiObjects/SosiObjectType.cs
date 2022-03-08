using DiBK.Gml2Sosi.Application.Attributes;
using DiBK.Gml2Sosi.Application.Extensions;
using DiBK.Gml2Sosi.Application.Models.Geometries;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public abstract class SosiObjectType : SosiElement
    {
        [SosiProperty("..OBJTYPE", 0)]
        public abstract string ObjType { get; }
        [SosiProperty("..IDENT", 1)]
        public Ident Ident { get; set; } = new();
        [SosiProperty("..FØRSTEDIGITALISERINGSDATO", 2)]
        public string FørsteDigitaliseringsdato { get; set; }
        [SosiProperty("..OPPDATERINGSDATO", 3)]
        public string Oppdateringsdato { get; set; }
        [SosiProperty("..KVALITET", 4)]
        public string Kvalitet { get; set; }
        public CartographicElementType ElementType { get; set; }
        public override string ElementName => $"{ElementType.GetSosiName()} {SequenceNumber}";
        public List<SosiPoint> Points { get; set; }

        public override async Task WriteToStreamAsync(StreamWriter streamWriter)
        {
            SetSosiValues();

            await streamWriter.WriteLineAsync(ElementName + ":");

            foreach (var value in SosiValues)
                await streamWriter.WriteLineAsync(value);
        }
    }
}
