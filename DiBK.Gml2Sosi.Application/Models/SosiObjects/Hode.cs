using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Application.Models.SosiObjects
{
    public class Hode : SosiElement
    {
        [SosiProperty("..TEGNSETT", 0)]
        public string Tegnsett { get; } = "UTF-8";
        [SosiProperty("..TRANSPAR", 1)]
        public Transpar Transpar { get; set; }
        [SosiProperty("..OMRÅDE", 2)]
        public Område Område { get; set; }
        [SosiProperty("..SOSI-VERSJON", 3)]
        public string SosiVersjon { get; } = "4.5";
        [SosiProperty("..SOSI-NIVÅ", 4)]
        public string SosiNivå { get; } = "4";
        [SosiProperty("..OBJEKTKATALOG", 5)]
        public string Objektkatalog { get; } = "Regplanforslag 20190401 * Arealplan Reguleringsplanforslag";
        public override string ElementName => ".HODE";

        public override void WriteToStream(StreamWriter streamWriter)
        {
            SetSosiValues();

            streamWriter.WriteLine(ElementName);

            foreach (var value in SosiValues)
                streamWriter.WriteLine(value);
        }
    }
}
