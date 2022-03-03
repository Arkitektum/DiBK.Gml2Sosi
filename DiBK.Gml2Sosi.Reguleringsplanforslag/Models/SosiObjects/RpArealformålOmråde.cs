using DiBK.Gml2Sosi.Application.Attributes;

namespace DiBK.Gml2Sosi.Reguleringsplanforslag.Models.SosiObjects
{
    [FeatureMember("RpArealformålOmråde")]
    public class RpArealformålOmråde : RpfSosiSurfaceObject
    {
        public override string ObjType { get; } = "RpArealformålOmråde";
        [SosiProperty("..RPAREALFORMÅL", 7)]
        public string Arealformål { get; set; }
        [SosiProperty("..EIERFORM", 8)]
        public string Eierform { get; set; }
        [SosiProperty("..UTNYTT", 9)]
        public Utnytting Utnytting { get; set; }
        [SosiProperty("..BESKRIVELSE", 10)]
        public string Beskrivelse { get; set; }
        [SosiProperty("..FELTNAVN", 11)]
        public string Feltnavn { get; set; }
        [SosiProperty("..UTEAREAL", 12)]
        public string Uteoppholdsareal { get; set; }
        [SosiProperty("..BYGGVERK", 13)]
        public string Byggverkbestemmelse { get; set; }
        [SosiProperty("..AVKJ", 14)]
        public string Avkjørselsbestemmelse { get; set; }
    }
}
